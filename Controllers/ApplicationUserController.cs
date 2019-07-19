using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnBank.models;

namespace UnBank.Controllers
{
    [Route("api/ApplicationUser")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        [Route("Register")]
        //POST: api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationsUserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                //UserName é tratado como numero da conta nesta aplicação.
                UserName = model.N_Conta,
                Email = model.Email,
                Nome = model.Nome,
                Cep = model.Cep,
                Cpf = model.Cpf,
                PhoneNumber = model.PhoneNumber
            };
            //password não é especificado aqui pois deve ser incriptado.
            try
            {
                //tenta criar o usuario com o uso da função CreateAsync
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
