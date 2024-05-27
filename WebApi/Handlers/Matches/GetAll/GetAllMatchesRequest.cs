using Domain.Features.Matches.Models;
using Domain.Features.Matches.Profiles;
using MediatR;
using Soap.Models;

namespace WebApi.Handlers.Matches.GetAll;

public class GetAllMatchesRequest : IRequest<List<MatchModel>>
{
}