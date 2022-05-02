using Demo.WebAPI.Exceptions;
using Demo.WebAPI.Models;
using MongoDB.Driver;

namespace Demo.WebAPI.DataBase;

public interface IWithDBRaceConditionRepository
{
    Task AddAsync(CounterVersion entity);
    Task<CounterVersion> GetAsync(Guid id);
    Task UpdateAsync(CounterVersion entity);
}

public class WithDBRaceConditionRepository : IWithDBRaceConditionRepository
{
    private const string COLLECTION_NAME = nameof(CounterVersion);

    private readonly IMongoDBContext _context;
    private readonly IMongoCollection<CounterVersion> _collection;


    public WithDBRaceConditionRepository(IMongoDBContext context)
    {
        _context = context;
        _collection = _context.GetCollection<CounterVersion>(COLLECTION_NAME);
    }


    public async Task AddAsync(CounterVersion entity)
    {
        entity.Version = Guid.NewGuid();
        await _collection.InsertOneAsync(entity);
    }

    public async Task<CounterVersion> GetAsync(Guid id)
        => await _collection.FindSync(f => f.Id == id).SingleOrDefaultAsync();


    public async Task UpdateAsync(CounterVersion entity)
    {
        var currenctVersion = entity.Version;

        entity.Version = Guid.NewGuid();

        var response = await _collection.ReplaceOneAsync(
            r => r.Id == entity.Id && r.Version == currenctVersion,
            entity,
            new ReplaceOptions { IsUpsert = false }
        );

        if(response.ModifiedCount == 0)
        {
            throw new DBRaceConditionException();
        }
    }
}
