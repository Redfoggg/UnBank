using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UnBank.models;

namespace UnBank.Controllers
{
    [Route("api/ApplicationUser")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;
        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }
        [HttpPost]
        [Route("Register")]
        //POST: api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationsUserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                //UserName é tratado como Cpf nesta aplicação.
                UserName = model.Cpf,
                Email = model.Email,
                Nome = model.Nome,
                Cep = model.Cep,
                N_conta = model.N_Conta,
                PhoneNumber = model.PhoneNumber
            };
            //password não é especificado aqui pois deve ser encriptado.
            try
            {
                //tenta criar o usuario com o uso da função CreateAsync, método await por ser um método Async
                var result = await _userManager.CreateAsync(applicationUser, model.Senha);
                return Ok(result);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        [HttpPost]
        [Route("Login")]
        //POST: api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Cpf);
            //autenticação do usuário, verificação de usuário e senha.
            if(user != null &&await _userManager.CheckPasswordAsync(user, model.Senha))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
                        new Claim("UserID", user.Id.ToString())
                    }),
                    //token expira X tempo depois de gerado
                    Expires = DateTime.UtcNow.AddDays(1),
                    //chave secreta
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256)
                };
                //cria o token seguindo as configurações acima
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new {token});
            }
            else
                return BadRequest(new {message = "Número da conta ou senha incorretos"});
        }
    }
}
