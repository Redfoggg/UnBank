using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnBank.models
{
    //Propriedades customizadas do IdentityUser
    public class ApplicationUser : IdentityUser
    {
        public string Nome {get; set;}
        //public string Cpf {get; set;}
        public string N_conta {get; set;}
        public string Cep {get; set;}
        public decimal Saldo {get; set;}
    }
}