using Masa.Admin.Domain.Entities;
using Masa.Admin.Domain.Repositories;

namespace Masa.Admin.WebApi.Infrastructure.Repositories
{
    public class LogExceptionRepository : Repository<MasaAdminDbContext, LogException>, ILogExceptionRepository
    {
        private readonly MasaAdminDbContext _context;
        public LogExceptionRepository(MasaAdminDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _context = context;
        }
    }
}
