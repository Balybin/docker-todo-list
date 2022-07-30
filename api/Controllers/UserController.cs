using docker_todo_list.Features.Users;
using DockerTodoList.Domain;
using DockerTodoList.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace docker_todo_list.Controllers
{
   

    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMediator _mediator;

        public UserController(DatabaseContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IReadOnlyCollection<GetUsersResponse>> GetUsers([FromQuery] GetUsersQuery query, CancellationToken token)
        {
            return await _mediator.Send(query, token);
        }
        [HttpPost]
        public async Task<Guid> CreateUser([FromBody] CreateUserCommand command, CancellationToken token)
        {
            return await _mediator.Send(command, token);
        }
    }
}
