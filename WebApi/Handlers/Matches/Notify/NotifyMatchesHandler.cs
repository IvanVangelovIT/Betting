using Domain.Constants;
using Domain.DbContexts;
using Domain.Features.Matches.Models;
using Domain.Features.Matches.Profiles;
using MediatR;
using Newtonsoft.Json;
using Soap.Models;

namespace WebApi.Handlers.Matches.Notify;

public class NotifyMatchesHandler : IRequestHandler<NotifyMatchesRequest, MatchesChangeTracker>
{
    private readonly RedisContext _redisContext;

    public NotifyMatchesHandler(RedisContext redisContext)
    {
        this._redisContext = redisContext;
    }
    
    public async Task<MatchesChangeTracker> Handle(NotifyMatchesRequest request, CancellationToken cancellationToken)
    {
        var redisDb = this._redisContext.GetDatabase();

        var key = await redisDb.StringGetAsync(RedisConstants.ChangeTracker);
        
        var notifyMatches = await redisDb.StringGetAsync(key.ToString());

        var parsedNotifyMatches = JsonConvert.DeserializeObject<MatchesChangeTracker>(notifyMatches);

        return parsedNotifyMatches;
    }
}