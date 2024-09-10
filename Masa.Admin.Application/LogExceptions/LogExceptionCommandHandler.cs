using Masa.Admin.Application.LogExceptions.Commands;
using Masa.Admin.Domain.Entities;
using Masa.Admin.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Masa.Admin.Application.LogExceptions
{
    public class LogExceptionCommandHandler
    {
        private readonly ILogger<LogExceptionCommandHandler> _logger;

        private readonly ILogExceptionRepository _logExceptionRepository;


        public LogExceptionCommandHandler(ILogger<LogExceptionCommandHandler> logger, ILogExceptionRepository logExceptionRepository)
        {
            _logger = logger;
            _logExceptionRepository = logExceptionRepository;
        }

        [EventHandler]
        public async Task CreateAsync(CreateLogExceptionCommand command)
        {
            var entity = command.Map<LogException>();
            await _logExceptionRepository.AddAsync(entity);
        }
    }
}
