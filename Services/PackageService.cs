using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.Package;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class PackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;

        public PackageService(IPackageRepository packageRepository, IMapper mapper)
        {
            _packageRepository = packageRepository;
            _mapper = mapper;
        }

        public async Task<PackageDto> CreatePackageAsync(int createdBy, CreatePackageRequestDto request)
        {
            try
            {
                Console.WriteLine($"Creating package: {request.Name} by user {createdBy}");
                
                var package = _mapper.Map<Package>(request);
                package.CreatedBy = createdBy;
                package.CreatedAt = DateTime.UtcNow;
                
                Console.WriteLine($"Package mapped: {package.Name}, CreatedBy: {package.CreatedBy}");
                
                var createdPackage = await _packageRepository.AddAsync(package);
                
                Console.WriteLine($"Package created with ID: {createdPackage.PackageId}");
                
                return _mapper.Map<PackageDto>(createdPackage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating package: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PackageDto>> GetActivePackagesAsync()
        {
            var packages = await _packageRepository.GetActivePackagesAsync();
            return _mapper.Map<IEnumerable<PackageDto>>(packages);
        }

        public async Task<PackageDto?> GetPackageAsync(int id)
        {
            var package = await _packageRepository.GetByIdAsync(id);
            return package != null ? _mapper.Map<PackageDto>(package) : null;
        }

        public async Task DeletePackageAsync(int id)
        {
            var package = await _packageRepository.GetByIdAsync(id);
            if (package != null)
            {
                package.IsActive = false;
                package.UpdatedAt = DateTime.UtcNow;
                await _packageRepository.UpdateAsync(package);
            }
        }
    }
} 