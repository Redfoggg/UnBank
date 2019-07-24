using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnBank.service;

namespace UnBank.models
{
    //modelo para acesso das propriedades necessárias para cadastro de um novo usuario(conta)
    public class ApplicationsUserModel 
    {
        public string Email  { get; set; }
        public string Senha { get; set; }
        public string Nome {get; set;}
        public long Cpf {get; set;}
        
        public long Cep {get; set;}
        public string PhoneNumber  { get; set; }
        
        public string N_Conta = NumeroContaService.Gerar_N_Conta();

    }
}