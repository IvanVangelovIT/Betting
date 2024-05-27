using Domain.Features.Matches.Models;
using Domain.Features.Matches.Profiles;
using MediatR;

namespace WebApi.Handlers.Matches.GetById;

public class GetMatchByIdRequest : IRequest<MatchModel>
{
    public int MatchId { get; private set; }

    public void SetMatchId (int matchId) => this.MatchId = matchId;
}