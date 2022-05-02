using Demo.WebAPI.DataBase;
using Demo.WebAPI.Exceptions;
using Demo.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;

namespace Demo.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WithPollyController : ControllerBase
{
    private const int MAX_RETRIES = 20;
    private readonly Random _random = new();
    private readonly AsyncRetryPolicy<IActionResult> _retryPolicy;

    private readonly IWithDBRaceConditionRepository _repository;

    public WithPollyController(IWithDBRaceConditionRepository repository)
    {
        _retryPolicy = Policy<IActionResult>
            .Handle<DBRaceConditionException>()
                .WaitAndRetryAsync(
                    MAX_RETRIES,
                    times => TimeSpan.FromMilliseconds(_random.Next(1, MAX_RETRIES * 10)));

        _repository = repository;
    }


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
        => await _retryPolicy.ExecuteAsync(async () =>
        {
            var counter = await _repository
               .GetAsync(id);

            if(counter == null)
            {
                return NotFound();
            }

            counter.Value++;
            await _repository
                .UpdateAsync(counter);

            var response = await _repository
                .GetAsync(id);

            return Ok(response.Value);
        });
}
