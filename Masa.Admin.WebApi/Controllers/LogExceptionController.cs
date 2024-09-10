

using Masa.Admin.Application.LogExceptions.Commands;
using Masa.Admin.Application.LogExceptions.Queries;

namespace Masa.Admin.WebApi.Controllers
{
    /// <summary>
    /// 异常日志控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LogExceptionController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        private readonly IUserContext _userContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="userContext"></param>
        public LogExceptionController(IEventBus eventBus, IUserContext userContext)
        {
            _eventBus = eventBus;
            _userContext = userContext;
        }

        /// <summary>
        /// 获取异常日志列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResult> GetLogExceptionList(LogExceptionQuery query)
        {
            await _eventBus.PublishAsync(query);
            return Results.Ok(query.Result);
        }

        /// <summary>
        /// 创建异常日志
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResult> CreateLogException(CreateLogExceptionCommand command)
        {
            await _eventBus.PublishAsync(command);
            return Results.Ok(command.Id);
        }
    }
}
