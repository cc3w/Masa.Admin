using Masa.Admin.Domain.enums;

namespace Masa.Admin.Application.Users.Commands
{
    public record UpdateUserCommand(Guid Id) : Command
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; } = null!;

        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 邮箱
        /// </summaryfasfdasdf
        public string? Email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenederType GenederType { get; set; }

    }
}
