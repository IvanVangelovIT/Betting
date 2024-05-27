using Domain.Constants;
using Domain.DbContexts;
using Domain.Features.Matches.MatchesChangeTracker;
using Domain.Features.Matches.Models;
using Domain.Features.Sports.Services;
using Newtonsoft.Json;
using Soap.Http;
using Soap.Models;
using StackExchange.Redis;

namespace WebApi.Jobs.Hangfire;

public class ProcessSportsEveryOneMinuteJob
{
    private readonly HttpService httpService;
    private readonly ParseXmlElements parseXmlElements;
    private readonly RedisContext redisContext;
    private readonly SportsChangeTrackerService sportsChangeTracker;
    private readonly SportsDbSaverService sportsDbSaverService;

    private readonly string esportUrl =
        "https://sports.ultraplay.net/sportsxml?clientKey=9C5E796D-4D54-42FD-A535-D7E77906541A&sportId=2357&days=7";

    public ProcessSportsEveryOneMinuteJob(
        HttpService httpService,
        ParseXmlElements parseXmlElements,
        RedisContext redisContext,
        SportsDbSaverService sportsDbSaverService,
        SportsChangeTrackerService sportsChangeTracker)
    {
        this.httpService = httpService;
        this.parseXmlElements = parseXmlElements;
        this.redisContext = redisContext;
        this.sportsChangeTracker = sportsChangeTracker;
        this.sportsDbSaverService = sportsDbSaverService;
    }

    public async Task ExecuteAsync()
    {
       var betting = await this.httpService.FetchXmlDataAsync(esportUrl);

        var parsedCurrentSports = this.parseXmlElements.ParseXml<XmlSportsModel>(betting);

        await this.sportsDbSaverService.SaveToBettingDbAsync(parsedCurrentSports);

        var redisDb = this.redisContext.GetDatabase();

        // Get previous sports cache and set current cache
        var (previousSportsKey, cachedPreviousSports) =
            await GetAndSetPreviousCacheAsync(parsedCurrentSports, redisDb, RedisConstants.CurrentSport);

        // Delete previous sports cache
        await DeletePreviousCacheAsync(previousSportsKey, redisDb);

        // Get previous change tracker sports cache 
        var (previousMatchTrackerKey, cachedPreviousMatchTracker) =
            await this.GetPreviousCacheAsync<MatchesChangeTracker>(redisDb, RedisConstants.ChangeTracker);
        
        // Set current change tracker cache 
        await this.SetChangeTrackerSportsAsync(parsedCurrentSports, cachedPreviousSports, redisDb);

        // Delete previous change tracker cache
        await DeletePreviousCacheAsync(previousMatchTrackerKey, redisDb);
    }

    private async Task<(string Key, XmlSportsModel Data)> GetAndSetPreviousCacheAsync(XmlSportsModel data, IDatabase db, string cacheKey)
    {
        var (key, previousCache) = await this.GetPreviousCacheAsync<XmlSportsModel>(db, cacheKey);
            
        await SetRedisAsync(data, db, RedisConstants.CurrentSport);
            
        return (key, previousCache);
    }

    private async Task DeletePreviousCacheAsync(string cacheKey, IDatabase db)
    {
        if (!string.IsNullOrEmpty(cacheKey))
        {
            await this.DeleteCacheByKeyAsync(cacheKey, db);
        }
    }
    
    private async Task DeleteCacheByKeyAsync(string key, IDatabase db)
    {
        if (key is null) { return; }
        
        await db.KeyDeleteAsync(key);
    }

    private async Task SetChangeTrackerSportsAsync(
        XmlSportsModel parsedCurrentSports,
        XmlSportsModel cachedPreviousSports,
        IDatabase db)
    {
        if (parsedCurrentSports is null || cachedPreviousSports is null) { return; }

        var (addedMatches, removedMatches, changedMatches)
            = sportsChangeTracker.GetChangeTracker<MatchXmlModel>(
                parsedCurrentSports,
                cachedPreviousSports,
                pm => pm.Sports
                    .SelectMany(sport => sport.Events)
                    .SelectMany(ev => ev.Matches),
                s => s.MatchId);
        
        var (addedBets, removedBets, changedBets)
            = sportsChangeTracker.GetChangeTracker<BetXmlModel>(
                parsedCurrentSports,
                cachedPreviousSports,
                pm => pm.Sports
                    .SelectMany(sport => sport.Events)
                    .SelectMany(ev => ev.Matches)
                    .SelectMany(x => x.Bets),
                s => s.BetId);
        
        var (addedOdds, removedOdds, changedOdds)
            = sportsChangeTracker.GetChangeTracker<OddXmlModel>(
                parsedCurrentSports,
                cachedPreviousSports,
                pm => pm.Sports
                    .SelectMany(sport => sport.Events)
                    .SelectMany(ev => ev.Matches)
                    .SelectMany(x => x.Bets)
                    .SelectMany(x => x.Odds),
                s => s.OddId);

        var matchChangeTracker = CreateMatchChangeTracker(SportType.Match, addedMatches, removedMatches, changedMatches);
        
        var betChangeTracker = CreateMatchChangeTracker(SportType.Bet, addedBets, removedBets, changedBets);
        
        var oddsChangeTracker = CreateMatchChangeTracker(SportType.Odd, addedOdds, removedOdds, changedOdds);

        var allTrackers = new MatchesChangeTracker();
        
        allTrackers.MatchesChanges.AddRange(matchChangeTracker);
        
        allTrackers.MatchesChanges.AddRange(betChangeTracker);
        
        allTrackers.MatchesChanges.AddRange(oddsChangeTracker);   
        
        await this.SetRedisAsync<MatchesChangeTracker>(allTrackers, db, RedisConstants.ChangeTracker);
    }
    
    private List<MatchChangeTracker> CreateMatchChangeTracker<T>(SportType sportType, IEnumerable<T> addedItems, IEnumerable<T> removedItems, IEnumerable<T> changedItems) where T : IHasId
    {
        var trackers = new List<MatchChangeTracker>();

        if (addedItems.Any())
        {
            trackers.Add(new MatchChangeTracker
            {
                SportType = sportType,
                ChangeType = ChangeType.Add,
                Visibility = Visibility.Show,
                Ids = addedItems.Select(x => x.Id).ToList()
            });
        }

        if (removedItems.Any())
        {
            trackers.Add(new MatchChangeTracker
            {
                SportType = sportType,
                ChangeType = ChangeType.Remove,
                Visibility = Visibility.Hide,
                Ids = removedItems.Select(x => x.Id).ToList()
            });
        }

        if (changedItems.Any())
        {
            trackers.Add(new MatchChangeTracker
            {
                SportType = sportType,
                ChangeType = ChangeType.Change,
                Visibility = Visibility.Show,
                Ids = changedItems.Select(x => x.Id).ToList()
            });
        }

        return trackers;
    }

    private async Task<(string key, T)> GetPreviousCacheAsync<T>(IDatabase db, string key)
        where T : class
    {
        var previousSportsKey = await db.StringGetAsync(key);

        if (previousSportsKey.IsNullOrEmpty)
        {
            return (null, null);
        }

        var previousSportsXml = await db.StringGetAsync(previousSportsKey.ToString());

        if (previousSportsXml.IsNullOrEmpty)
        {
            return (null, null);
        }

        return (previousSportsKey, JsonConvert.DeserializeObject<T>(previousSportsXml));
    }

    private async Task SetRedisAsync<T>(T currentData, IDatabase db, string key)
        where T : class
    {
        var json = JsonConvert.SerializeObject(currentData);

        var currentKey = $"{key}_{DateTime.UtcNow}";

        await db.StringSetAsync(currentKey, json);

        await db.StringSetAsync(key, currentKey);
    }
}