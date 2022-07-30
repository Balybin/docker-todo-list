using DockerTodoList.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace docker_todo_list.Features.Todos
{
    public class RemoveTodoCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid TodoListItemId { get; set; }
    }
    public class RemoveTodoCommandHandler : IRequestHandler<RemoveTodoCommand, Unit>
    {
        private readonly DatabaseContext _dbContext;

        public RemoveTodoCommandHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(RemoveTodoCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.SingleAsync(user => user.Id == request.UserId, cancellationToken);

            var todoListItem = await _dbContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .SingleAsync(todoListItem => todoListItem.Id == request.TodoListItemId, cancellationToken);

            user.RemoveTodoListItem(todoListItem);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
