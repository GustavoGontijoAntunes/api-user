using app.Domain.Models.Enum;

namespace app.WebApi.Dtos.Results
{
    public class AuditResult : BaseModelResult
    {
        public DateTime DateTime { get; set; }
        public AuditType Type { get; set; }
        public string User { get; set; }
        public string TableName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string ChangedColumns { get; set; }
    }
}