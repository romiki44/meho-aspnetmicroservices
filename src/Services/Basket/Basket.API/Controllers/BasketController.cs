using Basket.API.Entities;
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

        public BasketController(IBasketRepository basketRepo)
        {
            this.basketRepo = basketRepo;
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
            var updatedBasket=await basketRepo.UpdateBasket(basket);
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
