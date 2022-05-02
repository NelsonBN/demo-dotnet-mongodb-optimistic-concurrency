using Demo.WebAPI.MediatorDemo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class WithMediatRController : ControllerBase
{
    private readonly IMediator _mediator;

    public WithMediatRController(IMediator mediator)
        => _mediator = mediator;


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetQuery(id)));


    [HttpPost]
    public async Task<IActionResult> Add()
        => Ok(await _mediator.Send(new AddCommand()));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new UpdateCommand(id));
        return response is null ? NotFound() : Ok();
    }
}
