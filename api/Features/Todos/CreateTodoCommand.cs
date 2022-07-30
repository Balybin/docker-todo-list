using DockerTodoList.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace docker_todo_list.Features.Todos
{
    public class CreateTodoCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
    }

    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Guid>
    {
        private readonly DatabaseContext _dbContext;

        public CreateTodoCommandHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.SingleAsync(user => user.Id == request.UserId, cancellationToken);

            var todoListItem = user.AddTodoListItem(request.Title);
            todoListItem.setContent(request.Content ?? string.Empty);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return todoListItem.Id;
        }
    }
}
