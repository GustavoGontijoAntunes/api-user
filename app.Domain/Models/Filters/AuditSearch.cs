using app.Domain.Extensions;
using app.Domain.Models.Enum;

namespace app.Domain.Models.Filters
{
    public class AuditSearch : PagedSearch
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AuditType Type { get; set; }
        public string User { get; set; }
        public string TableName { get; set; }
    }
}