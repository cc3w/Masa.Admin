using Masa.Admin.Application.LogExceptions.Commands;
using Masa.Admin.Common.Http;
using Masa.Admin.Common.Result.Helper;
using Microsoft.AspNetCore.Http.Features;
using System.Runtime.ExceptionServices;

namespace Masa.Admin.WebApi.Extensions
{
    /// <summary>
    /// 处理异常中间件
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private IEventBus _eventBus = default!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <param name="webHostEnvironment"></param>
        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // 中间件是单例服务，IEventBus是作用域服务（在请求的作用域内创建），所以需要通过 HttpContext.RequestServices 获取服务实例
            _eventBus = context.RequestServices.GetRequiredService<IEventBus>();

            ExceptionDispatchInfo exceptionDispatchInfo;
            try
            {
                await _next(context);
                return;
            }
            catch (Exception ex)
            {

                if (_webHostEnvironment.IsDevelopment())
                {
                    throw;
                }

                // 捕获异常，但不在 catch 块中继续处理，因为这样不利于堆栈的使用
                exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
            }

            await HandleExceptionAsync(context, exceptionDispatchInfo);
        }

        private async Task HandleExceptionAsync(HttpContext context, ExceptionDispatchInfo exceptionDispatchInfo)
        {

            Guid eventId = await PublishEventAsync(exceptionDispatchInfo.SourceException, context.User.Identity?.Name);

            if (context.Response.HasStarted)
            {
                _logger.LogError(exceptionDispatchInfo.SourceException, $"EventId: {eventId}. Message: HTTP响应已经开始，无法处理该异常");
                return;
            }

            _logger.LogError(exceptionDispatchInfo.SourceException, $"EventId: {eventId}. Message: 全局异常拦截");


            context.Response.Clear();
            context.SetEndpoint(endpoint: null);
            var routeValuesFeature = context.Features.Get<IRouteValuesFeature>();
            if (routeValuesFeature != null)
            {
                routeValuesFeature.RouteValues = null!;
            }

            // 响应头处理
            context.Response.Headers.CacheControl = "no-cache,no-store";
            context.Response.Headers.Pragma = "no-cache";
            context.Response.Headers.Expires = "-1";
            context.Response.Headers.ETag = default;

            // 响应
            var result = ApiResultHelper.Result500InternalServerError($"系统异常，异常Id: {eventId}，请联系管理员");
            await context.Response.WriteAsync(result);
            await HttpResponseHelper.WriteAsync(context.Response, result);
        }

        /// <summary>
        /// 发布异常事件
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="operationUser"></param>
        /// <returns></returns>
        private async Task<Guid> PublishEventAsync(Exception exception, string? operationUser)
        {

            // 定义异常事件模型
            var command = new CreateLogExceptionCommand()
            {
                OperationUser = operationUser,
                Name = exception.Message,
                Message = exception.Message,
                ClassName = exception.TargetSite?.DeclaringType?.FullName,
                MethodName = exception.TargetSite?.Name,
                ExceptionSource = exception.Source,
                StackTrace = exception.StackTrace,
                Parameters = exception.TargetSite?.GetParameters().ToString(),
                ExceptionTime = DateTime.Now
            };

            // 发布异常事件
            await _eventBus.PublishAsync(command);

            return command.Id;
        }
    }
}
