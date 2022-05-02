using Demo.WebAPI.DataBase;
using MediatR;

namespace Demo.WebAPI.MediatorDemo;

public class UpdateCommand : IRequest<int?>
{
    public Guid Id { get; init; }

    public UpdateCommand(Guid id)
        => Id = id;



    internal class Handler :
        IRequestHandler<UpdateCommand, int?>
    {
        private readonly IWithDBRaceConditionRepository _repository;

        public Handler(IWithDBRaceConditionRepository repository)
            => _repository = repository;

        public async Task<int?> Handle(UpdateCommand command, CancellationToken cancellationToken)
        {
            var counter = await _repository
               .GetAsync(command.Id);

            if(counter == null)
            {
                return default;
            }

            counter.Value++;
            await _repository
                .UpdateAsync(counter);

            var response = await _repository
                .GetAsync(command.Id);

            return response.Value;
        }
    }
}
