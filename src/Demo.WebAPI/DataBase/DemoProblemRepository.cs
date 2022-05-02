using Demo.WebAPI.Models;
using MongoDB.Driver;

namespace Demo.WebAPI.DataBase;

public interface IDemoProblemRepository
{
    Task AddAsync(Counter entity);
    Task<Counter> GetAsync(Guid id);
    Task UpdateAsync(Counter entity);
}

public class DemoProblemRepository : IDemoProblemRepository
{
    private const string COLLECTION_NAME = nameof(Counter);

    private readonly IMongoDBContext _context;
    private readonly IMongoCollection<Counter> _collection;


    public DemoProblemRepository(IMongoDBContext context)
    {
        _context = context;
        _collection = _context.GetCollection<Counter>(COLLECTION_NAME);
    }


    public async Task AddAsync(Counter entity)
        => await _collection.InsertOneAsync(entity);

    public async Task<Counter> GetAsync(Guid id)
        => await _collection.FindSync(f => f.Id == id).SingleOrDefaultAsync();


    public async Task UpdateAsync(Counter entity)
        => await _collection.ReplaceOneAsync(
            r => r.Id == entity.Id,
            entity,
            new ReplaceOptions { IsUpsert = false }
        );
}
