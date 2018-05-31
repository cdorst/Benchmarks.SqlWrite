using System.ComponentModel.DataAnnotations.Schema;

namespace Benchmarks.Model
{
    [Table(nameof(EntityA))]
    public class EntityA
    {
        public int Id { get; set; }

        public EntityB EntityB { get; set; }
        public short EntityBId { get; set; }

        public EntityC EntityC { get; set; }
        public long EntityCId { get; set; }

        public object ToSqlSaveParameters() => new { EntityBId, EntityCId };
    }
}
