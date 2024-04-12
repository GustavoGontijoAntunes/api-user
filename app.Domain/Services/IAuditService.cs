using app.Domain.Extensions;
using app.Domain.Models.Audit;
using app.Domain.Models.Filters;

namespace app.Domain.Services
{
    public interface IAuditService
    {
        PagedList<Audit> Get(AuditSearch search);
    }
}