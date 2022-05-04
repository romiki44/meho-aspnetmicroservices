using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepo;
        private readonly DiscountGrpcService discountGrpcService;

        public BasketController(IBasketRepository basketRepo, DiscountGrpcService discountGrpcService)
        {
            this.basketRepo = basketRepo;
            this.discountGrpcService = discountGrpcService;
        }

        [HttpGet("{username}", Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBasket(string username)
        {
            var basket = await basketRepo.GetBasket(username);
            if (basket == null)
                basket = new ShoppingCart(username);

            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // TODO : Communicate with Discount.Grpc
            // and Calculate latest prices of product into shopping cart

            // consume Discount Grpc
            foreach(var item in basket.Items)
            {
                var coupon=await discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            var updatedBasket =await basketRepo.UpdateBasket(basket);
            return Ok(updatedBasket);
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await basketRepo.DeleteBasket(username);
            return Ok();
        }
    }
}
