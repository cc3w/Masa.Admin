using Masa.Admin.Domain.enums;

namespace Masa.Admin.Domain.Models
{
    public record UserQueryResult
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 岗位Id
        /// </summary>
        public Guid? PositionId { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid? DepartId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 盐
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

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

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountType AccountType { get; set; }
    }
}
