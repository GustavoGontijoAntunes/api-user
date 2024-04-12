using app.Domain.Adapter;
using app.Domain.Exceptions;
using app.Domain.Extensions;
using app.Domain.Models;
using app.Domain.Models.Excel;
using app.Domain.Repositories;
using app.Domain.Resources;
using app.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace app.Application
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IExcelAdapter _excelAdapter;

        public PermissionService(IPermissionRepository permissionRepository, IExcelAdapter excelAdapter)
        {
            _permissionRepository = permissionRepository;
            _excelAdapter = excelAdapter;
        }

        public IEnumerable<Permission> GetAll()
        {
            return _permissionRepository.GetAll();
        }

        public Permission GetByName(string name)
        {
            return _permissionRepository.GetByName(name);
        }

        public Permission GetById(long id)
        {
            return _permissionRepository.GetById(id);
        }

        public void Add(Permission permission, string userName)
        {
            permission.ThrowIfNotValid();
            var permissionAlreadyExsists = GetByName(permission.Name);

            if (permissionAlreadyExsists != null)
            {
                throw new DomainException(string.Format(CustomMessages.PermissionAlreadyExists, permission.Name));
            }

            _permissionRepository.Add(permission);
            SaveChanges(userName);
        }

        public void Update(Permission permission, string userName)
        {
            permission.ThrowIfNotValid();
            var existingPermission = GetById(permission.Id);
            var permissionNameAlreadyExsists = GetByName(permission.Name);

            if (permissionNameAlreadyExsists != null && permissionNameAlreadyExsists.Id != permission.Id)
            {
                throw new DomainException(string.Format(CustomMessages.PermissionAlreadyExists, permission.Name));
            }

            if (existingPermission == null)
            {
                throw new DomainException(CustomMessages.PermissionIdNotExists);
            }

            _permissionRepository.Update(permission);
            SaveChanges(userName);
        }

        public void AddOrUpdateRange(List<Permission> permissions, string userName)
        {
            foreach (var item in permissions)
            {
                item.ThrowIfNotValid();
                var permissionAlreadyExsists = GetByName(item.Name);

                if (permissionAlreadyExsists != null)
                {
                    throw new DomainException(string.Format(CustomMessages.PermissionAlreadyExists, item.Name));
                }
            }

            _permissionRepository.AddOrUpdateRange(permissions);
            SaveChanges(userName);
        }

        public void DeleteById(long id, string userName)
        {
            var permission = GetById(id);

            if (permission == null)
            {
                throw new DomainException(CustomMessages.PermissionIdNotExists);
            }

            _permissionRepository.DeleteById(id);
            SaveChanges(userName);
        }

        public List<Permission> GetPermissionsByProfileId(long profileId)
        {
            return _permissionRepository.GetPermissionsByProfileId(profileId);
        }

        public Excel GetExcel()
        {
            var permissions = _permissionRepository.GetAll();
            permissions.ThrowIfNullOrEmpty();

            return _excelAdapter.GetPermission(permissions);
        }

        public Excel GetExcelModel()
        {
            return _excelAdapter.GetPermissionModel();
        }

        public void RegisterByExcel(IFormFile file, string userName)
        {
            var permissions = _excelAdapter.ReadPermission(file);
            permissions.ThrowIfNullOrEmpty();

            _permissionRepository.AddOrUpdateRange(permissions);
            SaveChanges(userName);
        }

        private void SaveChanges(string userName)
        {
            _permissionRepository.SaveChanges(userName);
        }
    }
}