using app.Domain.Extensions;
using app.Domain.Models.Audit;
using app.Domain.Models.Enum;
using app.Domain.Models.Filters;
using app.Domain.Repositories;
using app.Domain.Resources;
using app.Domain.Services;
using Microsoft.Extensions.Localization;

namespace app.Application
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IStringLocalizer<CustomMessages> _customMessagesLocalizer;
        private readonly IStringLocalizer<CustomTables> _customTablesLocalizer;

        public AuditService(IAuditRepository auditRepository,
            IStringLocalizer<CustomMessages> customMessagesLocalizer,
            IStringLocalizer<CustomTables> customTablesLocalizer)
        {
            _auditRepository = auditRepository;
            _customMessagesLocalizer = customMessagesLocalizer;
            _customTablesLocalizer = customTablesLocalizer;
        }

        public PagedList<Audit> Get(AuditSearch search)
        {
            var result = _auditRepository.Get(search);

            foreach (var item in result.Items)
            {
                item.TypeString = GetAuditTypeString(item.Type);
                item.TableName = _customTablesLocalizer[item.TableName];
            }

            return result;
        }

        private string GetAuditTypeString(AuditType type)
        {
            switch (type)
            {
                case AuditType.Create:
                    return _customMessagesLocalizer["Create"];
                case AuditType.Update:
                    return _customMessagesLocalizer["Update"];
                case AuditType.Delete:
                    return _customMessagesLocalizer["Delete"];
                default:
                    return _customMessagesLocalizer["None"];
            }
        }
    }
}