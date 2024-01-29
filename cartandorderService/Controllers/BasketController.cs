using cartandorderService.Entities;
using cartandorderService.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cartandorderService.Controllers
{
    [ApiController]
    [Route("cartapi/[controller]")]
   [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly ICartRepository cart;

        public BasketController(ICartRepository cart)
        {
            this.cart = cart;
        }
       
        [HttpGet("mycart")]
        public async Task<ActionResult<List<CartToReturnDto>>> GetCart()
        {
            try
            {
                 var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var cartList = await cart.GetCart(email);
                // Log the count and contents of cartList for debugging
                Console.WriteLine($"CartList Count: {cartList.Count}");
                foreach (var cartToReturnDto in cartList)
                {
                    Console.WriteLine($"Cart: {cartToReturnDto.cart}");
                    Console.WriteLine($"Product: {cartToReturnDto.prod}");
                }
                return Ok(cartList);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("addcart")]
        public async Task<int> AddCart(Cart prod)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            prod.CreatedBy = email;
            var cartProd = await cart.AddCart(prod);
            return cartProd;
        }
        [HttpDelete("delcart/{id}")]
        public async Task DeleteCart(int id)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            await cart.Delete(id, email);
        }
        [HttpPut("Updatecart")]
        public async Task Update(int productId, Cart prod)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            await cart.edit(productId, email, prod);
        }
        [HttpPost("placeorder")]
        public async Task<ActionResult> PlaceOrder(Cart order)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            order.CreatedBy = email;
            var ord = await cart.order(order);
            if (ord) return Ok();
            return BadRequest();

        }
        [HttpGet("getorders")]
        public async Task<ActionResult<List<OrderReturnDto>>> GetOrder()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return Ok(await cart.getOrder(email));
        }
    }
}
