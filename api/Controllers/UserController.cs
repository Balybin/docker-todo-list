using DockerTodoList.Domain;
using DockerTodoList.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace docker_todo_list.Controllers
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TodoItemsCount { get; set; }
    }

    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public UserController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet()]
        public async Task<IReadOnlyCollection<UserViewModel>> GetUsers(string? keywords, CancellationToken token)
        {
            var query = _dbContext.Users.AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(user => user.Name.ToLower().Contains(keywords.ToLower()));
            }

            return await query
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    TodoItemsCount = user.TodoListItems.Count()
                }).ToListAsync(token);
        }
        [HttpPost]
        public async Task<Guid> CreateUser(string name, CancellationToken token)
        {
            var user = new User(name);
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync(token);
            return user.Id;
        }
    }
}
