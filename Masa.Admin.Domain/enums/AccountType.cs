using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masa.Admin.Domain.enums
{
    public enum AccountType
    {
        [Description("超级管理员")]
        SuperAdmin, 

        [Description("管理员")]
        Admin, 

        [Description("普通用户")]
        None,
    }
}
