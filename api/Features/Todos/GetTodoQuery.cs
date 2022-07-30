using DockerTodoList.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace docker_todo_list.Features.Todos
{
    public class GetTodoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
    public class GetTodoQuery :IRequest<IReadOnlyCollection<GetTodoResponse>>
    {
        public Guid userId { get; set; }
    }

    public class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, IReadOnlyCollection<GetTodoResponse>>
    {
        private DatabaseContext _dbContext;

        public GetTodoQueryHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<GetTodoResponse>> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.SingleAsync(user => user.Id == request.userId, cancellationToken);
            return await _dbContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .Select(todoListItem => new GetTodoResponse
                {
                    Content = todoListItem.Content,
                    Title = todoListItem.Title,
                    Id = todoListItem.Id
                }).ToListAsync(cancellationToken);
        }
    }
}
