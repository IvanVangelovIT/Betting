using AutoMapper;
using Domain.DbContexts;
using Domain.Features.Bets.Entities;
using Domain.Features.Events.Entities;
using Domain.Features.Matches.Entities;
using Domain.Features.Odds.Entities;
using Domain.Features.Sports.Entities;
using Domain.Features.Sports.Profiles;
using Soap.Models;

namespace Domain.Features.Sports.Services;

public class SportsDbSaverService
{
    public readonly BettingDbContext dbContext;
    public static Mapper _mapper;


    public SportsDbSaverService(
        BettingDbContext dbContext)
    {
        this.dbContext = dbContext;
        _mapper = SportProfile.InitializeAutomapper();
    }
    
    public async Task SaveToBettingDbAsync(XmlSportsModel xmlSportses)
    {
        var tasks = xmlSportses.Sports
            .Where(s => s is not null)
            .Chunk(1)
            .SelectMany(chunk => chunk.Select(sport => this.AddSportAsync(sport)))
            .ToList();

        await Task.WhenAll(tasks);
        
        await this.dbContext.SaveChangesAsync();
    }

    private async Task AddSportAsync(SportXmlModel sportXmlModel)
    {
        var sportEntity = _mapper.Map<SportEntity>(sportXmlModel);

        await this.dbContext.Sports.AddAsync(sportEntity);

        var eventTasks = sportXmlModel.Events
            .Select(xmlEvent => AddEventAsync(sportEntity, xmlEvent))
            .ToList();
        
        await Task.WhenAll(eventTasks);
    }

    private async Task AddEventAsync(SportEntity sport, EventXmlModel eventXmlModel)
    {
        var eventEntity = _mapper.Map<EventEntity>(eventXmlModel);

        eventEntity.Sport = sport;

        await this.dbContext.Events.AddAsync(eventEntity);

        var matchTasks = eventXmlModel.Matches
            .Select(xmlMatch => AddMatchAsync(eventEntity, xmlMatch))
            .ToList();

        await Task.WhenAll(matchTasks);
    }

    private async Task AddMatchAsync(EventEntity eventEntity, MatchXmlModel matchXmlModel)
    {
        var matchEntity = _mapper.Map<MatchEntity>(matchXmlModel);

        matchEntity.Event = eventEntity;

        await this.dbContext.Matches.AddAsync(matchEntity);

        var betTasks = matchXmlModel.Bets
            .Select(xmlBet => AddBetAsync(matchEntity, xmlBet))
            .ToList();

        await Task.WhenAll(betTasks);
    }

    private async Task AddBetAsync(MatchEntity matchEntity, BetXmlModel betXmlModel)
    {
        var betEntity = _mapper.Map<BetEntity>(betXmlModel);

        betEntity.Match = matchEntity;

        await this.dbContext.Bets.AddAsync(betEntity);

        var oddTasks = betXmlModel.Odds
            .Select(xmlOdd => AddOddAsync(betEntity, xmlOdd))
            .ToList();

        await Task.WhenAll(oddTasks);
    }

    private async Task AddOddAsync(BetEntity betEntity, OddXmlModel oddXmlModel)
    {
        var oddEntity = _mapper.Map<OddEntity>(oddXmlModel);

        oddEntity.Bet = betEntity;

        await this.dbContext.Odds.AddAsync(oddEntity);
    }
}