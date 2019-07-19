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
        public long Cpf {get; set;}
        //public int N_conta {get; set;}
        public long Cep {get; set;}
        public decimal Saldo {get; set;}
    }
}