using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ApiControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        //[HttpPost]
        //[Route("SendEmail")]
        //public async Task<IActionResult> SendMailAsync(SendEmailConfirmationDto sendEmailConfirmationDto, CancellationToken cancellationToken)
        //{
        //    await _emailService.SendEmailWithAttachmentAsync(new SendEmailConfirmationDto()
        //    {
        //        Email = sendEmailConfirmationDto.Email,
        //    });

        //    return Ok();

        //}
    }
}
