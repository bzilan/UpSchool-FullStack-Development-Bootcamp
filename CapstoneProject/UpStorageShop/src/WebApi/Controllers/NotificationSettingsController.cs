using Application.Common.Dtos;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Authorize]
    public class NotificationSettingsController : ApiControllerBase
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public NotificationSettingsController(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public IActionResult GetNotificationSettings(string userId)
        {
            var settings = _applicationDbContext.NotificationSettings
            .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId).Result;

            if (settings == null)
            {
                return BadRequest("Bildirim ayarları bulunamadı.");
            }
            var settingsDto = new NotificationSettingsDto()
            {
                PushNotification = settings.PushNotification,
                EmailNotification = settings.EmailNotification,
                EmailAddress = settings.EmailAddress
            };

            return Ok(settings);
        }

        [HttpPut("Update")]
        public IActionResult UpdateNotificationSettings(NotificationSettingsDto settingsDto)
        {
            var settings = _applicationDbContext.NotificationSettings
            .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId).Result;

            settings.PushNotification = settingsDto.PushNotification;
            settings.EmailNotification = settingsDto.EmailNotification;
            settings.EmailAddress = settingsDto.EmailAddress;

            _applicationDbContext.NotificationSettings.Update(settings);
            _applicationDbContext.SaveChanges();

            return Ok(settings);
        }
    }
}
