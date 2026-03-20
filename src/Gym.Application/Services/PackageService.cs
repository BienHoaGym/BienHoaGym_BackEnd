using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Packages;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;

namespace Gym.Application.Services;

public class PackageService : IPackageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PackageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<PackageDto>> GetByIdAsync(Guid id)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);

        if (package == null || package.IsDeleted)
        {
            return ResponseDto<PackageDto>.FailureResult("Package not found");
        }

        var packageDto = _mapper.Map<PackageDto>(package);
        return ResponseDto<PackageDto>.SuccessResult(packageDto);
    }

    public async Task<ResponseDto<List<PackageDto>>> GetAllAsync()
    {
        var packages = await _unitOfWork.Packages.GetAllAsync();
        var activePackages = packages.Where(p => !p.IsDeleted).OrderBy(p => p.Price).ToList();

        var packageDtos = _mapper.Map<List<PackageDto>>(activePackages);
        return ResponseDto<List<PackageDto>>.SuccessResult(packageDtos);
    }

    public async Task<ResponseDto<PackageDto>> CreateAsync(CreatePackageDto dto)
    {
        var package = _mapper.Map<MembershipPackage>(dto);
        package.IsActive = true;

        await _unitOfWork.Packages.AddAsync(package);
        await _unitOfWork.SaveChangesAsync();

        var packageDto = _mapper.Map<PackageDto>(package);
        return ResponseDto<PackageDto>.SuccessResult(packageDto, "Package created successfully");
    }

    public async Task<ResponseDto<PackageDto>> UpdateAsync(Guid id, UpdatePackageDto dto)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);

        if (package == null || package.IsDeleted)
        {
            return ResponseDto<PackageDto>.FailureResult("Package not found");
        }

        _mapper.Map(dto, package);
        package.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Packages.Update(package);
        await _unitOfWork.SaveChangesAsync();

        var packageDto = _mapper.Map<PackageDto>(package);
        return ResponseDto<PackageDto>.SuccessResult(packageDto, "Package updated successfully");
    }
   
    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);

        if (package == null || package.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Package not found");
        }

        // Check if package has active subscriptions
        var subscriptions = await _unitOfWork.Subscriptions.FindAsync(s =>
            s.PackageId == id &&
            !s.IsDeleted);

        if (subscriptions.Any())
        {
            return ResponseDto<bool>.FailureResult("Cannot delete package with active subscriptions. Please deactivate instead.");
        }

        // Soft delete
        package.IsDeleted = true;
        package.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Packages.Update(package);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Package deleted successfully");
    }

    public async Task<ResponseDto<List<PackageDto>>> GetActivePackagesAsync()
    {
        var packages = await _unitOfWork.Packages.GetActivePackagesAsync();
        var packageDtos = _mapper.Map<List<PackageDto>>(packages);
        return ResponseDto<List<PackageDto>>.SuccessResult(packageDtos);
    }
}