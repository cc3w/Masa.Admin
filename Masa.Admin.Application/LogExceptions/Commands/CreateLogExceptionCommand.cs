﻿namespace Masa.Admin.Application.LogExceptions.Commands
{
    public record CreateLogExceptionCommand : Command
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string? OperationUser { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string? MethodName { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        public string? ExceptionSource { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string? Parameters { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public DateTime ExceptionTime { get; set; }
    }
}
