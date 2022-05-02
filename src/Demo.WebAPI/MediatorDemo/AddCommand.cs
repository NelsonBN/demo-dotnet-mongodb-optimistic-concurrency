using Demo.WebAPI.DataBase;
using Demo.WebAPI.Models;
using MediatR;

namespace Demo.WebAPI.MediatorDemo;

public class AddCommand : IRequest<Guid>
{
    internal class Handler :
        IRequestHandler<AddCommand, Guid>
    {
        private readonly IWithDBRaceConditionRepository _repository;

        public Handler(IWithDBRaceConditionRepository repository)
            => _repository = repository;

        public async Task<Guid> Handle(AddCommand command, CancellationToken cancellationToken)
        {
            var counter = new CounterVersion();
            counter.Id = Guid.NewGuid();
            counter.Value = 0;


            await _repository
                .AddAsync(counter);


            return counter.Id;
        }
    }
}
