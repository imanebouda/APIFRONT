using ITKANSys_api.Core.Dtos;
using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace ITKANSys_api.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedRolesAsync();
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> Login(LoginDto record);
        Task<DataSourceResult> PasswordForgotten(Object record);
        Task<DataSourceResult> Search(Object record);
        Task<DataSourceResult> Insert(Object record);
        Task<DataSourceResult> Update(Object record);
        Task<DataSourceResult> Delete(Object record);
        Task<DataSourceResult> UpdatePassWord(Object record);
        Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto);
        Task<UserDto> GetAllAsync();
        Task<UserDto> GetAllByRoleAsync();
        Task<UserDto> GetAllCoPiloteAsync();
        Task<UserDto> GetAllPiloteAsync();
        Task<UserDto> GetAllAuditeur();


    }
}

