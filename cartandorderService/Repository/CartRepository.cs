
using Business_Layer;
using cartandorderService.Entities;
using IdentityModel.Client;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace cartandorderService.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly StoreContext context;
        private readonly IHttpClientFactory httpClientFactory;
        string apiUrl = "http://localhost:5000/prodapi/product/";
        public CartRepository(StoreContext context,IHttpClientFactory httpClientFactory)
        {
            this.context = context;
            this.httpClientFactory = httpClientFactory;
        }
        private async Task<ProductToReturn> GetProducts(int productId)
        {
            var serverClient = httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("http://localhost:5002/");//well known address
            var tokenRes = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest { 
            Address=discoveryDocument.TokenEndpoint,
            ClientId="Client_Id",
            ClientSecret="client_secret",
            Scope="ApiOne"
            });
            string productEndpointUrl = apiUrl + productId;
            var apiClient = httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenRes.AccessToken);
            var prod1=await apiClient.GetFromJsonAsync<ProductToReturn>(productEndpointUrl);
            Console.WriteLine(tokenRes.AccessToken);

            //var httpClient = new HttpClient();
            //var prod1 = await httpClient.GetFromJsonAsync<ProductToReturn>(productEndpointUrl);
            return prod1;
        }
        private async Task UpdateProduct(ProductToReturn prod)
        {
            string productEndpointUrl = apiUrl +"editquan/"+ prod.Id;
            var httpClient = new HttpClient();
           // var json = JsonSerializer.Serialize(prod);
           // var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await httpClient.PutAsJsonAsync(productEndpointUrl, prod);

        }
        public async Task<int> AddCart(Cart cart)
        {
            int productId = cart.ProductId;

            var prod1 = await GetProducts(productId);

            var prod = await context.Cart.FirstOrDefaultAsync(x => x.ProductId == cart.ProductId && x.CreatedBy == cart.CreatedBy);
            // var prod1 = await context.Products.FirstOrDefaultAsync(x => x.Id == cart.ProductId);
            if (prod == null)
            {
                await context.Cart.AddAsync(cart);

            }
            else
            {
                if (prod1.AvailableQuantity < prod.Quantity + cart.Quantity)
                {
                    prod.Quantity = prod1.AvailableQuantity;
                }
                else
                    prod.Quantity += cart.Quantity;
                
                context.Update(prod);


            }
            await context.SaveChangesAsync();
            return cart.Id;


        }
        public async Task<List<CartToReturnDto>> GetCart(string email)
        {
            var cartList = await context.Cart.Where(x => x.CreatedBy == email).ToListAsync();
            var cartListProper = new List<CartToReturnDto>();
            foreach(var i in cartList)
            {
                var cart = new CartToReturnDto();
                cart.cart = i;
                cart.prod = await GetProducts(i.ProductId);
                cartListProper.Add(cart);
            }
            return cartListProper;
        }
        public async Task Delete(int id, string email)
        {
            var prod = await context.Cart.FirstOrDefaultAsync(x => x.ProductId == id && x.CreatedBy == email);
            context.Remove(prod);
            await context.SaveChangesAsync();
        }
        public async Task edit(int productId, string email, Cart cart)
        {
            var prod = await context.Cart.FirstOrDefaultAsync(x => x.ProductId == productId && x.CreatedBy == email);
            var prod1 = await GetProducts(productId);

            if (prod1.AvailableQuantity - cart.Quantity < 0)
            { prod.Quantity = prod1.AvailableQuantity; }
            else
                prod.Quantity = cart.Quantity;
            context.Update(prod);
            await context.SaveChangesAsync();
        }
        public async Task<bool> order(Cart cart)
        {

            var prod = await GetProducts(cart.ProductId);
            if (prod.AvailableQuantity < cart.Quantity)
            {
                var delcart = await context.Cart.FirstOrDefaultAsync(x => x.ProductId == cart.ProductId && x.CreatedBy == cart.CreatedBy);
                context.Remove(delcart);
                await context.SaveChangesAsync();
                return false;
            }
            var ord = context.Order.FirstOrDefault(x => x.ProductId == cart.ProductId && x.OrderOf == cart.CreatedBy && x.Date.Date == DateTime.Now.Date);
            if (ord != null && ord.Date.Date == DateTime.Now.Date) { ord.Quantity += cart.Quantity; }
            else
            {
                var order = new Orders
                {
                    //PictureUrl = cart.PictureUrl,
                    OrderOf = cart.CreatedBy,
                    ProductId = cart.ProductId,
                    Date = DateTime.Now,
                   /// ProductBrand = cart.ProductBrand,
                    //ProductName = cart.Name,
                    Quantity = cart.Quantity

                };
                await context.Order.AddAsync(order);
            }
            prod.AvailableQuantity -= cart.Quantity;
            await UpdateProduct(prod);
            var cart1 = await context.Cart.FirstOrDefaultAsync(x => x.ProductId == cart.ProductId && x.CreatedBy == cart.CreatedBy);
            context.Remove(cart1);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<List<OrderReturnDto>> getOrder(string email)
        {
            var orders = await context.Order.Where(x => x.OrderOf == email).ToListAsync();
            var orderListProper = new List<OrderReturnDto>();
            foreach (var i in orders)
            {
                var order = new OrderReturnDto();
                order.orders = i;
                order.prod = await GetProducts(i.ProductId);
                orderListProper.Add(order);
            }
            return orderListProper;
            //return orders;
        }

    }
}
