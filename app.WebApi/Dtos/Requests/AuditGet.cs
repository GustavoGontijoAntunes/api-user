using app.Domain.Models.Enum;

namespace app.WebApi.Dtos.Requests
{
    public class AuditGet : Filter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AuditType Type { get; set; }
        public string User { get; set; }
        public string TableName { get; set; }
    }
}