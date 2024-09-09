using Masa.Admin.Application.Users.Commands;
using Masa.Admin.Domain.Entities;
using Masa.Admin.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Masa.Admin.Application.Users
{
    public class UserCommandHandler
    {
        private readonly ILogger<UserCommandHandler> _logger;

        private readonly IUserRepository _userRepository;
        public UserCommandHandler(ILogger<UserCommandHandler> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [EventHandler]
        public async Task CreateAsync(CreateUserCommand command)
        {
            var user = _userRepository.FindAsync(t => t.UserName.Equals(command.UserName));

            if (user != null)
            {
                throw new UserFriendlyException("用户名已存在");
            }

            var entity = command.Map<User>();
            await _userRepository.AddAsync(entity);
        }

        [EventHandler]
        public async Task UpdateAsync(UpdateUserCommand command)
        {
            var user = await _userRepository.FindAsync(t => t.Id == command.Id);

            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }

            var entity = command.Map<User>();
            await _userRepository.UpdateAsync(entity);
        }

        [EventHandler]
        public async Task DeleteAsync(DeleteUserCommand command)
        {
            var user = await _userRepository.FindAsync(t => t.Id == command.Id);

            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }

            await _userRepository.RemoveAsync(user);

        }
    }
}
