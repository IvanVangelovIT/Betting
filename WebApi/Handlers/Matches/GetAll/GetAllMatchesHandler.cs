using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Constants;
using Domain.DbContexts;
using Domain.Features.Matches.Models;
using Domain.Features.Matches.Profiles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Soap.Models;
using StackExchange.Redis;

namespace WebApi.Handlers.Matches.GetAll;

public class GetAllMatchesHandler : IRequestHandler<GetAllMatchesRequest, List<MatchModel>>
{
    private readonly BettingDbContext dbContext;
    private readonly RedisContext _redisContext;
    private static Mapper _mapper;
    private readonly string esportUrl = "https://sports.ultraplay.net/sportsxml?clientKey=9C5E796D-4D54-42FD-A535-D7E77906541A&sportId=2357&days=7";
    private readonly List<string> betNames = new List<string> { CommonConstants.Betting.MatchWinner, CommonConstants.Betting.MapAdvantage, CommonConstants.Betting.TotalMapsPlayed };


    public GetAllMatchesHandler(
        BettingDbContext dbContext,
        RedisContext redisContext)
    {
        this.dbContext = dbContext;
        this._redisContext = redisContext;
        _mapper = MatchProfile.InitializeAutomapper();

    }

    public async Task<List<MatchModel>> Handle(GetAllMatchesRequest request, CancellationToken cancellationToken)
    {
        var redisDb = this._redisContext.GetDatabase();

        var redisKey = await redisDb.StringGetAsync(RedisConstants.CurrentSport);

        Expression<Func<MatchModel, bool>> expressionPredicate = x => 
            (x.StartDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow.AddHours(24)) &&
            x.Bets.Any(b => 
                b.Name.ToLower() == CommonConstants.Betting.MatchWinner || 
                b.Name.ToLower() == CommonConstants.Betting.MapAdvantage || 
                b.Name.ToLower() == CommonConstants.Betting.TotalMapsPlayed);
        
        var matches = redisKey.IsNullOrEmpty
            ? await this.GetMatchesFromDbAsync(expressionPredicate)
            : await this.GetMatchesFromCacheAsync(redisDb, redisKey, expressionPredicate.Compile());

        matches = ProcessBetsAndOdds(matches);

        return matches;
    }

    private async Task<List<MatchModel>> GetMatchesFromCacheAsync(IDatabase redisDb, RedisValue sportsKey, Func<MatchModel, bool> predicate)
    {
        var sports = await redisDb.StringGetAsync(sportsKey.ToString());

        var xmlSportsModel = JsonConvert.DeserializeObject<XmlSportsModel>(sports);

        return _mapper.Map<List<MatchModel>>(xmlSportsModel.Sports
                .SelectMany(sport => sport.Events)
                .SelectMany(ev => ev.Matches))
            .Where(predicate)
            .ToList();
    }

    private async Task<List<MatchModel>> GetMatchesFromDbAsync(Expression<Func<MatchModel, bool>> predicate)
        => await this.dbContext
            .Sports
            .OrderByDescending(x => x.CreatedOn)
            .Take(1)
            .SelectMany(x => x.Events)
            .SelectMany(x => x.Matches)
            .ProjectTo<MatchModel>(MatchProfile.Configuration)
            .Where(predicate)
            .ToListAsync();

    private List<MatchModel> ProcessBetsAndOdds(List<MatchModel> matches)
    {
        var validBetNames = new HashSet<string>(betNames.Select(name => name.ToLower()));

        foreach (var match in matches)
        {
            RemoveInvalidBets(match, validBetNames);
            
            FilterAndGroupOdds(match);
        }

        return matches;
    }
    
    private void RemoveInvalidBets(MatchModel match, HashSet<string> validBetNames)
    {
        var betsToRemove = match.Bets.Where(bet => !validBetNames.Contains(bet.Name.ToLower())).ToList();

        foreach (var betToRemove in betsToRemove)
        {
            match.Bets.Remove(betToRemove);
        }
    }

    private void FilterAndGroupOdds(MatchModel match)
    {
        foreach (var bet in match.Bets)
        {
            var filteredOdds = bet.Odds.Where(o => o.SpecialBetValue != 0).ToList();
         
            if (filteredOdds.Any())
            {
                var filtereOdd = filteredOdds.GroupBy(o => o.SpecialBetValue)
                    .Select(g => g.First())
                    .First();

                bet.Odds = new List<OddXmlModel>() { filtereOdd };
            }
        }
    }
}