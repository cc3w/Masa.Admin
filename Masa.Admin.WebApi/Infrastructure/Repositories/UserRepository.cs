using Masa.Admin.Domain.Entities;
using Masa.Admin.Domain.Repositories;
using Masa.BuildingBlocks.Data.UoW;
using Masa.Contrib.Ddd.Domain.Repository.EFCore;

namespace Masa.Admin.WebApi.Infrastructure.Repositories
{
    public class UserRepository : Repository<MasaAdminDbContext, User>, IUserRepository
    {
        private readonly MasaAdminDbContext _context;

        public UserRepository(MasaAdminDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _context = context;
        }
    }
}
