using Microsoft.IdentityModel.Tokens;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.Auth;
using SmokingQuitSupportAPI.Models.DTOs.User;
using SmokingQuitSupportAPI.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BCrypt.Net;
using SmokingQuitSupportAPI.Models.Enums;

namespace SmokingQuitSupportAPI.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterUserRequestDto request)
        {
            return await RegisterAsync(request.Username, request.Email, request.Password, 
                request.ConfirmPassword, request.FullName, request.PhoneNumber, UserRole.User);
        }

        public async Task<AuthResponseDto> RegisterCoachAsync(RegisterCoachRequestDto request)
        {
            var response = await RegisterAsync(request.Username, request.Email, request.Password, 
                request.ConfirmPassword, request.FullName, request.PhoneNumber, UserRole.Coach);
            
            // Tạo coach application tự động được approve
            var coachApplication = new CoachApplication
            {
                UserId = response.User.UserId,
                Qualifications = request.Qualifications,
                Experience = request.Experience,
                Motivation = request.Specialization ?? "Registered as Coach",
                Status = "Approved",
                ApplicationDate = DateTime.UtcNow,
                ReviewedDate = DateTime.UtcNow
            };
            
            // Lưu coach application (cần inject ICoachApplicationRepository)
            
            return response;
        }

        public async Task<AuthResponseDto> RegisterAdminAsync(RegisterAdminRequestDto request)
        {
            // Kiểm tra mã admin (có thể lưu trong config)
            var validAdminCode = _configuration["AdminCode"] ?? "ADMIN123456";
            if (request.AdminCode != validAdminCode)
            {
                throw new InvalidOperationException("Mã xác thực admin không đúng");
            }
            
            return await RegisterAsync(request.Username, request.Email, request.Password, 
                request.ConfirmPassword, request.FullName, null, UserRole.Admin);
        }

        private async Task<AuthResponseDto> RegisterAsync(string username, string email, string password, 
            string confirmPassword, string? fullName, string? phoneNumber, UserRole role)
        {
            // Validate password confirmation
            if (password != confirmPassword)
            {
                throw new InvalidOperationException("Mật khẩu xác nhận không khớp");
            }

            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email này đã được đăng ký");
            }

            var existingUsername = await _userRepository.GetByUsernameAsync(username);
            if (existingUsername != null)
            {
                throw new InvalidOperationException("Tên đăng nhập này đã tồn tại");
            }

            // Create new user
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                FullName = fullName ?? string.Empty,
                Role = role.ToString()
            };

            var createdUser = await _userRepository.AddAsync(user);
            var token = GenerateJwtToken(createdUser);

            return new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(createdUser)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = GenerateJwtToken(user);
            var userDto = new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName ?? string.Empty,
                Role = user.Role
            };

            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 