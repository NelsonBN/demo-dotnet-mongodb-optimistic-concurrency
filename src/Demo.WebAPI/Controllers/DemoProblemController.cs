using Demo.WebAPI.Interfaces;
using Demo.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoProblemController : ControllerBase
{
    private readonly IDemoProblemRepository _repository;

    public DemoProblemController(IDemoProblemRepository repository)
        => _repository = repository;


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
        => Ok(await _repository.GetAsync(id));


    [HttpPost]
    public async Task<IActionResult> Add()
    {
        var counter = new Counter();
        counter.Id = Guid.NewGuid();
        counter.Value = 0;


        await _repository
            .AddAsync(counter);


        return Ok(counter.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put([FromRoute] Guid id)
    {
        var counter = await _repository
            .GetAsync(id);

        if(counter == null)
        {
            return NotFound(id);
        }


        counter.Value++;
        await _repository
            .UpdateAsync(counter);


        counter = await _repository
            .GetAsync(id);

        return Ok(counter.Value);
    }
}
