using Microsoft.AspNetCore.Mvc;

using Basket.Api.Entities;
using System.Net;
using Basket.Api.Repositories.Interfaces;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using AutoMapper;
namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _producer;


        public BasketController(IBasketRepository basketRepository, EventBusRabbitMQProducer producer, IMapper mapper)
        {
            _mapper = mapper;
            _producer = producer;
            _basketRepository = basketRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart?>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basket)
        {
            return Ok(await _basketRepository.UpdateBasket(basket));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            return Ok(await _basketRepository.DeleteBasket(userName));
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //get total price of basket
            //remove the basket
            //send checkout event to rabbitmq

            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            var basketRemoved = await _basketRepository.DeleteBasket(basketCheckout.UserName);

            if (!basketRemoved)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                _producer.PuplishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception)
            {

                throw;
            }

            return Accepted();

        }
    }
}
