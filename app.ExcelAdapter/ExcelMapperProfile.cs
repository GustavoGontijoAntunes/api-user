using app.Domain.Models;
using app.ExcelAdapter.Models;
using Profile = AutoMapper.Profile;

namespace app.ExcelAdapter
{
    public class ExcelMapperProfile : Profile
    {
        public ExcelMapperProfile()
        {
            #region Permission
            CreateMap<PermissionExcel, Permission>()
                .ForMember(to => to.Name,
                    source => source.MapFrom(from => from.name))
                .ForMember(to => to.Description,
                    source => source.MapFrom(from => from.description))
                ;
            #endregion
        }
    }
}