using MongoDB.Driver;

namespace Demo.WebAPI.Interfaces;

public interface IMongoDBContext : IDisposable
{
    IMongoCollection<T> GetCollection<T>(string collection);
}
