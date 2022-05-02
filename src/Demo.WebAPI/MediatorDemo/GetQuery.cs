using Demo.WebAPI.DataBase;
using Demo.WebAPI.Models;
using MediatR;

namespace Demo.WebAPI.MediatorDemo;

public record GetQuery : IRequest<CounterVersion>
{
    public Guid Id { get; init; }

    public GetQuery(Guid id)
        => Id = id;



    internal class Handler :
        IRequestHandler<GetQuery, CounterVersion>
    {
        private readonly IWithDBRaceConditionRepository _repository;

        public Handler(IWithDBRaceConditionRepository repository)
            => _repository = repository;

        public async Task<CounterVersion> Handle(GetQuery query, CancellationToken cancellationToken)
            => await _repository.GetAsync(query.Id);
    }
}
