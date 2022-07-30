using DockerTodoList.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace docker_todo_list.Controllers
{

    public class TodoListViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
    [ApiController]
    [Route("todo")]
    public class TodoController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public TodoController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IReadOnlyCollection<TodoListViewModel>> GetTodoListItems(Guid userId, CancellationToken token)
        {
            var user = await _dbContext.Users.SingleAsync(user => user.Id == userId, token);
            return await _dbContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .Select(todoListItem => new TodoListViewModel
                {
                    Content = todoListItem.Content,
                    Title = todoListItem.Title,
                    Id = todoListItem.Id
                }).ToListAsync(token);
        }

        [HttpPost]
        public async Task<Guid> CreateTodoListItem(Guid userId, string title, string? content, CancellationToken token)
        {
            var user = await _dbContext.Users.SingleAsync(user => user.Id == userId, token);

            var todoListItem = user.AddTodoListItem(title);
            todoListItem.setContent(content ?? string.Empty);

            await _dbContext.SaveChangesAsync(token);

            return todoListItem.Id;
        }
        [HttpDelete]
        public async Task RemoveTodoListItem(Guid userId, Guid todoListItemId, CancellationToken token)
        {
            var user = await _dbContext.Users.SingleAsync(user => user.Id == userId, token);

            var todoListItem = await _dbContext.Entry(user).Collection(user => user.TodoListItems).Query()
                .SingleAsync(todoListItem => todoListItem.Id == todoListItemId, token);

            user.RemoveTodoListItem(todoListItem);

            await _dbContext.SaveChangesAsync(token);

            return;
        }
    }
}
