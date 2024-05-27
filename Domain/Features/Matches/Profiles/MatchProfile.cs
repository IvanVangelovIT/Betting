using AutoMapper;
using Domain.Features.Bets.Entities;
using Domain.Features.Matches.Entities;
using Domain.Features.Matches.Models;
using Domain.Features.Odds.Entities;
using Soap.Models;

namespace Domain.Features.Matches.Profiles;

public class MatchProfile
{
    public static MapperConfiguration Configuration = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<MatchModel, MatchEntity>().ReverseMap();
        
        cfg.CreateMap<BetEntity, BetXmlModel>().ReverseMap();
        
        cfg.CreateMap<OddEntity, OddXmlModel>().ReverseMap();
        
        cfg.CreateMap<MatchModel, MatchXmlModel>().ReverseMap();
        
        cfg.CreateMap<BetEntity, BetXmlModel>().ReverseMap();
        
        cfg.CreateMap<OddEntity, OddXmlModel>().ReverseMap();
    });

    public static Mapper InitializeAutomapper()
    {
        var mapper = new Mapper(Configuration);
        
        return mapper;
    }
}