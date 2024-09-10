using Masa.Admin.Application.Users.Commands;
using Masa.Admin.Application.Users.Queries;


namespace Masa.Admin.WebApi.Controllers
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        private readonly IUserContext _userContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="userContext"></param>
        public UserController(IEventBus eventBus, IUserContext userContext)
        {
            _eventBus = eventBus;
            _userContext = userContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResult> GetUserList(UserQuery query)
        {
            await _eventBus.PublishAsync(query);
            return Results.Ok(query.Result);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResult> CreateUser(CreateUserCommand command)
        {
            await _eventBus.PublishAsync(command);
            return Results.Ok(command.Id);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResult> UpdateUser(UpdateUserCommand command)
        {
            await _eventBus.PublishAsync(command);
            return Results.Ok(command.Id);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResult> DeleteUser(DeleteUserCommand command)
        {
            await _eventBus.PublishAsync(command);
            return Results.Ok(command.Id);
        }
    }
}
