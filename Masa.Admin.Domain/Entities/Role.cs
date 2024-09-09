using Masa.BuildingBlocks.Ddd.Domain.Entities.Full;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masa.Admin.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
