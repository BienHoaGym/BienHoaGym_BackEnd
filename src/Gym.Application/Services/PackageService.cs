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
        try
        {
            var package = _mapper.Map<MembershipPackage>(dto);
            package.IsActive = true;
            
            // Đảm bảo Description không null nếu mapper không xử lý
            if (package.Description == null) package.Description = "";

            await _unitOfWork.Packages.AddAsync(package);
            await _unitOfWork.SaveChangesAsync();

            var packageDto = _mapper.Map<PackageDto>(package);
            return ResponseDto<PackageDto>.SuccessResult(packageDto, "Tạo gói tập thành công");
        }
        catch (Exception ex)
        {
            return ResponseDto<PackageDto>.FailureResult($"Lỗi khi tạo gói tập: {ex.Message}");
        }
    }

    public async Task<ResponseDto<PackageDto>> UpdateAsync(Guid id, UpdatePackageDto dto)
    {
        try
        {
            var package = await _unitOfWork.Packages.GetByIdAsync(id);

            if (package == null || package.IsDeleted)
            {
                return ResponseDto<PackageDto>.FailureResult("Không tìm thấy gói tập");
            }

            _mapper.Map(dto, package);
            package.UpdatedAt = DateTime.UtcNow;

            // Đảm bảo Description không null
            if (package.Description == null) package.Description = "";

            _unitOfWork.Packages.Update(package);
            await _unitOfWork.SaveChangesAsync();

            var packageDto = _mapper.Map<PackageDto>(package);
            return ResponseDto<PackageDto>.SuccessResult(packageDto, "Cập nhật gói tập thành công");
        }
        catch (Exception ex)
        {
            return ResponseDto<PackageDto>.FailureResult($"Lỗi khi cập nhật gói tập: {ex.Message}");
        }
    }
   
    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);

        if (package == null || package.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Không tìm thấy gói tập");
        }

        // Check if package has active subscriptions
        var subscriptions = await _unitOfWork.Subscriptions.FindAsync(s =>
            s.PackageId == id &&
            !s.IsDeleted);

        if (subscriptions.Any())
        {
            return ResponseDto<bool>.FailureResult("Không thể xóa gói tập đang có hội viên đăng ký sử dụng. Vui lòng chuyển trạng thái sang 'Ngừng bán'.");
        }

        // Soft delete
        package.IsDeleted = true;
        package.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Packages.Update(package);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Xóa gói tập thành công");
    }

    public async Task<ResponseDto<List<PackageDto>>> GetActivePackagesAsync()
    {
        var packages = await _unitOfWork.Packages.GetActivePackagesAsync();
        var packageDtos = _mapper.Map<List<PackageDto>>(packages);
        return ResponseDto<List<PackageDto>>.SuccessResult(packageDtos);
    }
}
