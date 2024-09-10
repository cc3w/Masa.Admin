using Masa.Admin.Application.LogExceptions.Queries;
using Masa.Admin.Domain.Entities;
using Masa.Admin.Domain.Models;
using Masa.Admin.Domain.Repositories;
using Masa.Utils.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Masa.Admin.Application.LogExceptions
{
    public class LogExceptionQueryHandler
    {
        private readonly ILogger<LogExceptionQueryHandler> _logger;

        private readonly ILogExceptionRepository _logExceptionRepository;

        public LogExceptionQueryHandler(ILogger<LogExceptionQueryHandler> logger, ILogExceptionRepository logExceptionRepository)
        {
            _logger = logger;
            _logExceptionRepository = logExceptionRepository;
        }

        [EventHandler]
        public async Task GetLogExceptionList(LogExceptionQuery query)
        {
            Expression<Func<LogException, bool>> cond = cond => true;

            if (!string.IsNullOrEmpty(query.OperationUser))
            {
                cond = cond.And(t => t.OperationUser.Contains(query.OperationUser));
            }

            var total = await _logExceptionRepository.GetCountAsync(cond);

            var roleList = await _logExceptionRepository.GetPaginatedListAsync(cond, (query.Page - 1) * query.PageSize, query.PageSize);

            var roleListDto = roleList.Map<List<LogExceptionQueryResult>>();

            query.Result = new PaginatedListBase<LogExceptionQueryResult>()
            {
                Total = total,
                TotalPages = (int)Math.Ceiling((double)total / query.PageSize),
                Result = roleListDto
            };
        }
    }
}
