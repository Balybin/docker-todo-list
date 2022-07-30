using DockerTodoList.Domain;
using DockerTodoList.Infrastructure.Database;
using FluentValidation;
using MediatR;

namespace docker_todo_list.Features.Users
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(5);
        }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly DatabaseContext _dbContext;

        public CreateUserCommandHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.Name);
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }

}
