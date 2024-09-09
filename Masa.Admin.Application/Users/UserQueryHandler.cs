using Masa.Admin.Application.Users.Queries;
using Masa.Admin.Domain.Entities;
using Masa.Admin.Domain.Models;
using Masa.Admin.Domain.Repositories;

using Masa.Utils.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Masa.Admin.Application.Users
{
    public class UserQueryHandler
    {
        private readonly ILogger<UserQueryHandler> _logger;

        private readonly IUserRepository _userRepository;

        public UserQueryHandler(ILogger<UserQueryHandler> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [EventHandler]
        public async Task GetUserList(UserQuery query)
        {
            Expression<Func<User, bool>> cond = cond => true;

            if (!string.IsNullOrEmpty(query.UserName))
            {
                cond = cond.And(t => t.UserName.Contains(query.UserName));
            }

            var total = await _userRepository.GetCountAsync(cond);

            var roleList = await _userRepository.GetPaginatedListAsync(cond, (query.Page - 1) * query.PageSize, query.PageSize);

            var roleListDto = roleList.Map<List<UserQueryResult>>();


            query.Result = new PaginatedListBase<UserQueryResult>()
            {
                Total = total,
                TotalPages = (int)Math.Ceiling((double)total / query.PageSize),
                Result = roleListDto
            };
        }
    }
}
