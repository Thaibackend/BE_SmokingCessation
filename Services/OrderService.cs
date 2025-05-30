using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.Order;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IPackageRepository packageRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _packageRepository = packageRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderRequestDto request, int userId)
        {
            var package = await _packageRepository.GetByIdAsync(request.PackageId);
            if (package == null)
                throw new ArgumentException("Package not found");

            var order = new Order
            {
                UserId = userId,
                PackageId = request.PackageId,
                Amount = package.Price,
                Status = "Pending",
                PaymentMethod = request.PaymentMethod,
                OrderDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdOrder = await _orderRepository.AddAsync(order);
            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto?> UpdateOrderStatusAsync(int id, string status, string? transactionId = null)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return null;

            order.Status = status;
            if (!string.IsNullOrEmpty(transactionId))
                order.TransactionId = transactionId;
            order.UpdatedAt = DateTime.UtcNow;

            var updatedOrder = await _orderRepository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            await _orderRepository.DeleteAsync(id);
            return true;
        }
    }
} 