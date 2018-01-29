using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationServer.Controllers.UserRegistration
{
    public class UserRegistrationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityServerInteractionService _interaction;

        public UserRegistrationController(IIdentityServerInteractionService interaction,
            IUserRepository userRepository)
        {
            _interaction = interaction;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult RegisterUser(string returnUrl)
        {
            var vm = new UserRegistrationViewModel()
            { ReturnUrl = returnUrl };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // create user + claims
                var userToCreate = new User()
                {
                    Username = model.Username,
                    Password = model.Password,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    Email = model.Email
                };
                userToCreate.Claims.Add(new UserClaim(JwtClaimTypes.Role, "user"));
                userToCreate.Claims.Add(new UserClaim(JwtClaimTypes.GivenName, model.Firstname));
                userToCreate.Claims.Add(new UserClaim(JwtClaimTypes.FamilyName, model.Lastname));
                userToCreate.Claims.Add(new UserClaim(JwtClaimTypes.Email, model.Email));
                userToCreate.Claims.Add(new UserClaim(JwtClaimTypes.Name, model.Username));

                // add the user through the API.
                using (var client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(userToCreate), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"http://localhost:53920/api/users", content);
                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        throw new Exception($"Creating a user failed.");
                    }
                }
                var createdUser = await _userRepository.GetByUsername(model.Username);

                // log the user in
                await HttpContext.SignInAsync(createdUser.SubjectId, createdUser.Username);

                // continue with the flow
                if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            // ModelState invalid, return the view with the passed-in model
            // so changes can be made
            return View(model);
        }
    }
}