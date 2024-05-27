using AutoMapper;
using Domain.Features.Bets.Entities;
using Domain.Features.Events.Entities;
using Domain.Features.Matches.Entities;
using Domain.Features.Odds.Entities;
using Domain.Features.Sports.Entities;
using Soap.Models;

namespace Domain.Features.Sports.Profiles;

public class SportProfile
{
    public static MapperConfiguration Configuration = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<SportEntity, SportXmlModel>().ReverseMap();
        
        cfg.CreateMap<EventEntity, EventXmlModel>().ReverseMap();
        
        cfg.CreateMap<MatchEntity, MatchXmlModel>().ReverseMap();
        
        cfg.CreateMap<BetEntity, BetXmlModel>().ReverseMap();
        
        cfg.CreateMap<OddEntity, OddXmlModel>().ReverseMap();
    });

    public static Mapper InitializeAutomapper()
    {
        var mapper = new Mapper(Configuration);
        
        return mapper;
    }
}