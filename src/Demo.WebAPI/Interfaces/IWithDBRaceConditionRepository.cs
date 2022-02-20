using Demo.WebAPI.Models;

namespace Demo.WebAPI.Interfaces;

public interface IWithDBRaceConditionRepository
{
    Task AddAsync(CounterVersion entity);
    Task<CounterVersion> GetAsync(Guid id);
    Task UpdateAsync(CounterVersion entity);
}
