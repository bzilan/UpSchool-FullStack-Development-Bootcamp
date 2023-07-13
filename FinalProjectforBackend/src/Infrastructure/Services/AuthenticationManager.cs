﻿//using Application.Common.Interfaces;
//using Application.Common.Models.Auth;
//using Domain.Identity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using System.ComponentModel.DataAnnotations;
//using FluentValidation;
//using FluentValidation.Results;
//using Application.Common.Localizations;
//using Microsoft.Extensions.Localization;

//namespace Infrastructure.Services
//{
//    public class AuthenticationManager : IAuthenticationService
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;
//        private readonly IJwtService _jwtService;


//        public AuthenticationManager(UserManager<User> userManager, SignInManager<User> signInManager, IJwtService jwtService)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _jwtService = jwtService;
//        }


//        public async Task<JwtDto> SocialLoginAsync(string email, string firstName, string lastName, CancellationToken cancellationToken)
//        {
//            var user = await _userManager.FindByEmailAsync(email);

//            if (user is not null)
//                return _jwtService.Generate(user.Id, user.Email, user.FirstName, user.LastName);

//            var userId = Guid.NewGuid().ToString();

//            user = new User()
//            {
//                Id = userId,
//                UserName = email,
//                Email = email,
//                EmailConfirmed = true,
//                FirstName = firstName,
//                LastName = lastName,
//                CreatedOn = DateTimeOffset.Now,
//                CreatedByUserId = userId,
//            };

//            var identityResult = await _userManager.CreateAsync(user);

//            if (!identityResult.Succeeded)
//            {
//                var failures = identityResult.Errors
//                    .Select(x => new ValidationFailure(x.Code, x.Description));

//                throw new ValidationException(failures);
//            }

//            return _jwtService.Generate(user.Id, user.Email, user.FirstName, user.LastName);
//        }
//    }
//}

