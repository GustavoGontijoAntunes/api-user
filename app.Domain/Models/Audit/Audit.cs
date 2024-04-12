using app.Domain.Models.Base;
using app.Domain.Models.Enum;

namespace app.Domain.Models.Audit
{
    public class Audit : BaseModel
    {
        public DateTime DateTime { get; set; }
        public AuditType Type { get; set; }
        public virtual string? TypeString { get; set; }
        public string User { get; set; }
        public string? TableName { get; set; }
        public string? KeyValues { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? ChangedColumns { get; set; }
    }
}