using Domain.Features.Matches.Models;
using MediatR;

namespace WebApi.Handlers.Matches.Notify;

public class NotifyMatchesRequest : IRequest<MatchesChangeTracker>
{
}