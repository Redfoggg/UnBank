using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UnBank.models;

namespace UnBank.service
{
    public class JwtService
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationSettings _appSettings;
        public JwtService(IOptions<ApplicationSettings> appSettings, UserManager<ApplicationUser> userManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
        }
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