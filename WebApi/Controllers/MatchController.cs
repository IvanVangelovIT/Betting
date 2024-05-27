using Domain.Features.Matches.Models;
using Domain.Features.Matches.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Soap.Models;
using WebApi.Handlers.Matches.GetAll;
using WebApi.Handlers.Matches.GetById;
using WebApi.Handlers.Matches.Notify;
namespace WebApi.Controllers;

[ApiController]
[Route("api/matches")]
public class AdxPricesController : ControllerBase
{
    private readonly IMediator mediator;

    public AdxPricesController(IMediator mediator)
        => this.mediator = mediator;
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<MatchModel>>> GetAll()
        => this.Ok(await this.mediator.Send(new GetAllMatchesRequest()));

    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MatchModel>> GetById([FromQuery] int id)
    {
        var request = new GetMatchByIdRequest();
        request.SetMatchId(id);
        return this.Ok(await this.mediator.Send(request));
    }

    [HttpGet("notify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<XmlSportsModel>> Notify()
        => this.Ok(await this.mediator.Send(new NotifyMatchesRequest()));
}