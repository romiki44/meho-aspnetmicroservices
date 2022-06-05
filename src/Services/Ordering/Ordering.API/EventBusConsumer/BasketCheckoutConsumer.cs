using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator mediator;
        private readonly ILogger<BasketCheckoutConsumer> logger;
        private readonly IMapper mapper;

        public BasketCheckoutConsumer(IMediator mediator, ILogger<BasketCheckoutConsumer> logger,
            IMapper mapper)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await mediator.Send(command);
            logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);
        }
    }
}
