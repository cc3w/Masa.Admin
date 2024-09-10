using Masa.Admin.Domain.Models;
using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Queries;
using Masa.Utils.Models;

namespace Masa.Admin.Application.LogExceptions.Queries
{
    public record LogExceptionQuery : Query<PaginatedListBase<LogExceptionQueryResult>>
    {
        /// <summary>
        /// 操作用户
        /// </summary>
        public string? OperationUser { get; set; }

        public int Page { get; set; } = default!;

        public int PageSize { get; set; } = default!;


        public bool IsRecycle { get; set; } = false;

        public override PaginatedListBase<LogExceptionQueryResult> Result { get; set; } = default!;
    }
}
