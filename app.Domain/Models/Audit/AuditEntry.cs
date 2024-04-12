using app.Domain.Models.Enum;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace app.Domain.Models.Audit
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public Audit ToAudit()
        {
            var audit = new Audit();
            audit.User = UserId;
            audit.Type = AuditType;
            audit.TypeString = AuditType.ToString();
            audit.TableName = TableName;
            audit.DateTime = DateTime.Now;
            audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.ChangedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);

            return audit;
        }
    }
}