using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.DbContexts;
using Domain.Features.Matches.Models;
using Domain.Features.Matches.Profiles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Handlers.Matches.GetById;

public class GetMatchByIdHandler : IRequestHandler<GetMatchByIdRequest, MatchModel>
{
    private readonly BettingDbContext dbContext;
    private static Mapper _mapper;

    public GetMatchByIdHandler(BettingDbContext dbContext)
    {
        this.dbContext = dbContext;
        _mapper = MatchProfile.InitializeAutomapper();
    }
    
    public async Task<MatchModel> Handle(GetMatchByIdRequest request, CancellationToken cancellationToken)
    {
        var match = await this.dbContext
            .Sports
            .OrderByDescending(x => x.CreatedOn)
            .Take(1)
            .SelectMany(x => x.Events)
            .SelectMany(x => x.Matches)
            .ProjectTo<MatchModel>(MatchProfile.Configuration)
            .FirstOrDefaultAsync(x => x.MatchId == request.MatchId);

        return match;
    }
}