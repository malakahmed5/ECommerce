using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketServices _basketServices;

        public BasketsController(IBasketServices basketServices)
        {
            _basketServices = basketServices;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDTO?>> GetBasket(string Id)
        {
            var basket = await _basketServices.GetBasket(Id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdateBasket(CustomerBasketDTO createOrUpdateasket)
        {
            var basket = await _basketServices.CreateOrUpdateBasket(createOrUpdateasket);
            return Ok(basket);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> DeleteBasket([FromRoute]string Id)
        {
            var result = await _basketServices.DeleteBasket(Id);
            return Ok(result);
        }

    }
}
