using Application.Common.Interfaces;
using Application.Features.Products.Commands.Add;
using Application.Features.Products.Queries.GetAll;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebApi.Hubs;

namespace WebApi.Controllers
{

    public class ProductsController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(ProductAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync(ProductGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        //[HttpDelete("{id:guid}")]
        //public IActionResult Delete(Guid id)
        //{
        //    var account = _applicationDbContext.Products.FirstOrDefault(x => x.Id == id);

        //    if (account is null) return NotFound("The selected account was not found.");

        //    _applicationDbContext.Products.Remove(account);
        //    _applicationDbContext.SaveChanges();

        //    return NoContent();
        //}
    }
}

