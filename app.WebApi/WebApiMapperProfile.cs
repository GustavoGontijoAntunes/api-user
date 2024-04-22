using app.Domain.Extensions;
using app.Domain.Models;
using app.Domain.Models.Authentication;
using app.Domain.Models.File;
using app.Domain.Models.Filters;
using app.WebApi.Dtos.Requests;
using app.WebApi.Dtos.Results;

namespace app.WebApi
{
    public class WebApiMapperProfile : AutoMapper.Profile
    {
        public WebApiMapperProfile()
        {        
            #region File Upload
            CreateMap<FileUpload, FileUploadPost>();
            CreateMap<FileDownload, FileDownloadResponse>();
            CreateMap<IFormFile, FileUpload>()
                .ForMember(to => to.FormFile, source => source.MapFrom(from => from));
            #endregion 

            #region Permission
            CreateMap<PermissionPost, Permission>();
            CreateMap<Permission, PermissionResult>();
            #endregion

            #region Profile
            CreateMap<ProfilePost, Profile>();
            CreateMap<Profile, ProfileResult>();
            CreateMap<ProfileGet, ProfileSearch>();
            CreateMap<PagedList<Profile>, CollectionResult<ProfileResult>>();
            #endregion

            #region User
            CreateMap<UserLogin, User>();
            CreateMap<UserPost, User>();
            CreateMap<UserPut, User>();
            CreateMap<UserPasswordPut, User>()
                .ForMember(to => to.Password,
                    source => source.MapFrom(from => from.OldPassword));
            CreateMap<User, UserResult>();
            CreateMap<UserGet, UserSearch>();
            CreateMap<PagedList<User>, CollectionResult<UserResult>>();
            #endregion
        }
    }
}