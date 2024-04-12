using app.Domain.Extensions;
using app.Domain.Models.Audit;
using app.Domain.Models.Filters;

namespace app.Domain.Repositories
{
    public interface IAuditRepository : IRepository<Audit>
    {
        PagedList<Audit> Get(AuditSearch search);
    }
}