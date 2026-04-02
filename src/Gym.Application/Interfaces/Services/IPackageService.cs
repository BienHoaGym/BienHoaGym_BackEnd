using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Packages;

namespace Gym.Application.Interfaces.Services;

public interface IPackageService
{
    Task<ResponseDto<PackageDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<List<PackageDto>>> GetAllAsync();
    Task<ResponseDto<PackageDto>> CreateAsync(CreatePackageDto dto);
    Task<ResponseDto<PackageDto>> UpdateAsync(Guid id, UpdatePackageDto dto);
    Task<ResponseDto<bool>> DeleteAsync(Guid id);
    Task<ResponseDto<List<PackageDto>>> GetActivePackagesAsync();
}
