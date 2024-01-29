using cartandorderService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cartandorderService
{
    public interface ICartRepository
    {
        Task<int> AddCart(Cart cart);
        Task Delete(int id, string email);
        Task edit(int productId, string email, Cart cart);
        Task<List<CartToReturnDto>> GetCart(string email);
        Task<List<OrderReturnDto>> getOrder(string email);
        Task<bool> order(Cart cart);
    }
}