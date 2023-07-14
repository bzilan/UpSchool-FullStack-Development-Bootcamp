using Application.Common.Models.WorkerService;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Remove;
using Application.Features.Orders.Commands.Update;
using Application.Features.Orders.Queries.GetAll;
using Application.Features.Orders.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class OrdersController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(OrderAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("CrawlerServiceExample")]
        public async Task<IActionResult> CrawlerServiceExampleAsync(WorkerServiceNewOrderAddedDto newOrderAddedDto)
        {
            return Ok();
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync(OrderGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(OrderUpdateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveAsync(OrderRemoveCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("GetById")]
        public async Task<IActionResult> GetByIdAsync(OrderGetByIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
