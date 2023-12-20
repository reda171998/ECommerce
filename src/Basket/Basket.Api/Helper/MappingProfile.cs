using AutoMapper;
using Basket.Api.Entities;
using EventBusRabbitMQ.Events;

namespace Basket.Api.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
