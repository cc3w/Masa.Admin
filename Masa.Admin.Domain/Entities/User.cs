using Masa.BuildingBlocks.Ddd.Domain.Entities.Full;

namespace Masa.Admin.Domain.Entities
{
    public class User : IFullEntity<Guid, Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        //public Guid? TenantId { get; set; }

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
