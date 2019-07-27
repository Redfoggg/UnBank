using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnBank.models;
using UnBank.service;

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
            //monstro criado em tentativas falhas que eu tive medo de destruir
           /*  string nCdEmpresa =""; 
            string sDsSenha   ="";      //   "<sDsSenha></sDsSenha>" 
            string nCdServico ="04014";           // "<nCdServico>04014</nCdServico>" 
            string sCepOrigem ="40393000";          // "<sCepOrigem>40393000</sCepOrigem>" 
            string sCepDestino = user.Cep;          // "<sCepDestino>14056724</sCepDestino>" 
            string nVlPeso ="1";          // "<nVlPeso>1</nVlPeso>" 
            int nCdFormato =3;          // "<nCdFormato>3</nCdFormato>"
            Decimal nVlComprimento =5;          // "<nVlComprimento>5</nVlComprimento>"
            Decimal nVlAltura =0;          // "<nVlAltura>0</nVlAltura>"
            Decimal nVlLargura =7;          // "<nVlLargura>7</nVlLargura>"
            Decimal nVlDiametro =3;          // "<nVlDiametro>3</nVlDiametro>"
            string sCdMaoPropria ="S";          // "<sCdMaoPropria>S</sCdMaoPropria>"
            Decimal nVlValorDeclarado =0;          // "<nVlValorDeclarado>0</nVlValorDeclarado>"
            string sCdAvisoRecebimento ="S";          // "<sCdAvisoRecebimento>S</sCdAvisoRecebimento>";
            var binding = new BasicHttpBinding() 
                {
                    Name = "BasicHttpBinding_CorreiosService",
                    MaxBufferSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647
                };
            var endpoint = new EndpointAddress("http://localhost:5000/api/UserProfile");
            var correios = new ServiceReference.CalcPrecoPrazoWSSoapClient(binding, endpoint);
            var consulta = correios.CalcPrecoPrazoAsync(nCdEmpresa, sDsSenha,nCdServico,sCepOrigem, sCepDestino,nVlPeso,nCdFormato,
            nVlComprimento,nVlAltura, nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento);
            */
            //consulta a api SOAP dos correios, res em XML 
            string consulta = Correios.CallWebService(user.Cep);
            //filtra a informação desejada na resposta xml
            String searchString = "<Valor>";
            int startIndex = consulta.IndexOf(searchString);
            searchString = "</" + searchString.Substring(1);
            int endIndex = consulta.IndexOf(searchString);
            String substring = consulta.Substring(startIndex, endIndex + searchString.Length - startIndex);
            XDocument doc = XDocument.Parse(substring);
            string valor = "";
            foreach (XElement valorElement in doc.Descendants("Valor"))
            {
                string valorValue = (string) valorElement;
                valor += valorValue;
            }
            return new 
            {
                user.Nome,
                user.Saldo,
                user.UserName,
                user.Cep,
                user.N_conta,
                user.PhoneNumber,
                valor
            };
        }
    }
}