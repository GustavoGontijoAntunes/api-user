using app.Domain.Extensions;
using app.Domain.Models.Audit;
using app.Domain.Models.Filters;
using app.Domain.Repositories;

namespace app.RepositoryAdapter.Repositories
{
    public class AuditRepository : BaseRepository<Audit>, IAuditRepository
    {
        public AuditRepository(AppDbContext context) : base(context)
        { }

        public PagedList<Audit> Get(AuditSearch search)
        {
            var query = Repository.AsQueryable();

            if (search.StartDate.HasValue)
            {
                query = query.Where(x => x.DateTime.Date >= search.StartDate.Value.Date);
            }

            if (search.EndDate.HasValue)
            {
                query = query.Where(x => x.DateTime.Date <= search.EndDate.Value.Date);
            }

            if (search.Type != null)
            {
                query = query.Where(x => x.Type == search.Type);
            }

            if (search.User != null)
            {
                query = query.Where(x => x.User.ToLower() == search.User.ToLower());
            }

            if (search.TableName != null)
            {
                query = query.Where(x => x.TableName.ToLower() == search.TableName.ToLower());
            }

            query = query.OrderByDescending(x => x.DateTime);

            return PagedList<Audit>.ToPagedList(query, search.PageIndex, search.PageSize);
        }
    }
}