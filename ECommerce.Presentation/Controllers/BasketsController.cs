using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketServices _basketServices;

        public BasketsController(IBasketServices basketServices)
        {
            _basketServices = basketServices;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDTO?>> GetBasket(string Id)
        {
            var result = await _basketServices.GetBasket(Id);
            return HandleResult(result)!;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdateBasket(CustomerBasketDTO createOrUpdateasket)
        {
            var result = await _basketServices.CreateOrUpdateBasket(createOrUpdateasket);
            return HandleResult(result);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> DeleteBasket([FromRoute]string Id)
        {
            var result = await _basketServices.DeleteBasket(Id);
            return result;
        }

    }
}
