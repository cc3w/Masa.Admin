﻿using Masa.Admin.Domain.enums;

namespace Masa.Admin.Domain.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : IFullEntity<Guid, Guid>
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
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; } = null!;

        /// <summary>
        /// 盐
        /// </summary>
        public string Salt { get; set; } = null!;

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
        public GenederType GenederType { get; set; } = GenederType.Unknown;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountType AccountType { get; set; } = AccountType.None;

        public bool IsDeleted { get; set; }

        public Guid Creator { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid Modifier { get; set; }

        public DateTime ModificationTime { get; set; }

        public IEnumerable<(string Name, object Value)> GetKeys()
        {
            yield return ("Id", Id);
        }
    }
}
