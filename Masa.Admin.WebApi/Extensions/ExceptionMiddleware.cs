using Masa.Admin.Application.LogExceptions.Commands;
using Masa.Admin.Common.Http;
using Masa.Admin.Common.Result.Helper;
using Microsoft.AspNetCore.Http.Features;
using System.Runtime.ExceptionServices;

namespace Masa.Admin.WebApi.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly Func<IServiceProvider, IEventBus> _eventBusFactory;


        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment webHostEnvironment,
            Func<IServiceProvider, IEventBus> eventBusFactory)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _eventBusFactory = eventBusFactory;

        }
        public async Task InvokeAsync(HttpContext context)
        {
            ExceptionDispatchInfo exceptionDispatchInfo = null;
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

            Guid eventId = await PublishEventAsync(context, exceptionDispatchInfo.SourceException, context.User.Identity?.Name);

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
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="operationUser"></param>
        /// <returns></returns>
        private async Task<Guid> PublishEventAsync(HttpContext context, Exception exception, string? operationUser)
        {
            IEventBus eventBus = _eventBusFactory(context.RequestServices);

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
            await eventBus.PublishAsync(command);

            return command.Id;
        }
    }
}
