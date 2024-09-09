using Masa.Admin.Domain.Models;
using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Queries;
using Masa.Utils.Models;

namespace Masa.Admin.Application.Users.Queries
{
    public record UserQuery : Query<PaginatedListBase<UserQueryResult>>
    {
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = default!;

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; } = default!;

        public string? UserName { get; set; }


        public bool IsRecycle { get; set; } = false;

        public override PaginatedListBase<UserQueryResult> Result { get; set; } = default!;
    }
}
