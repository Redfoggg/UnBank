using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnBank.models;

namespace UnBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        public UserProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        //GET: /api/UserProfile
        public async Task<Object> GetUserProfile()
        {
            //pega o UserID do Token JWT gerado
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new 
            {
                user.Nome,
                user.Saldo,
                user.UserName,
                user.Cep,
                user.N_conta,
                user.PhoneNumber
            };
        }
    }
}