using Demo.WebAPI.Models;

namespace Demo.WebAPI.Interfaces;

public interface IDemoProblemRepository
{
    Task AddAsync(Counter entity);
    Task<Counter> GetAsync(Guid id);
    Task UpdateAsync(Counter entity);
}
