using docker_todo_list.Features.Todos;
using DockerTodoList.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IMediator _mediator;

        public TodoController(DatabaseContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IReadOnlyCollection<GetTodoResponse>> GetTodoListItems([FromQuery] GetTodoQuery query, CancellationToken token)
        {
            return await _mediator.Send(query, token);
        }

        [HttpPost]
        public async Task<Guid> CreateTodoListItem([FromBody] CreateTodoCommand command, CancellationToken token)
        {
            return await _mediator.Send(command, token);
        }
        [HttpDelete]
        public async Task RemoveTodoListItem([FromBody] RemoveTodoCommand command, CancellationToken token)
        {
            await _mediator.Send(command, token);
            return;
        }
    }
}
