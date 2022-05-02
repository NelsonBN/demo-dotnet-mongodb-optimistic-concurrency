using Demo.WebAPI.DataBase;
using Demo.WebAPI.Exceptions;
using Demo.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WithDoWhileController : ControllerBase
{
    private const int MAX_RETRIES = 20;
    private readonly Random _random = new();

    private readonly IWithDBRaceConditionRepository _repository;

    public WithDoWhileController(IWithDBRaceConditionRepository repository)
        => _repository = repository;


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
        => Ok(await _repository.GetAsync(id));


    [HttpPost]
    public async Task<IActionResult> Add()
    {
        var counter = new CounterVersion();
        counter.Id = Guid.NewGuid();
        counter.Value = 0;


        await _repository
            .AddAsync(counter);


        return Ok(counter.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put([FromRoute] Guid id)
    {
        var attempt = 0;
        do
        {
            try
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

                break;
            }
            catch(DBRaceConditionException)
            {
                attempt++;
                Thread.Sleep(_random.Next(1, MAX_RETRIES * 10));
            }
        } while(attempt < MAX_RETRIES);

        var response = await _repository
            .GetAsync(id);

        return Ok(response.Value);
    }
}
