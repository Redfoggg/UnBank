using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnBank.service
{
    class NumeroContaService
    {
        public static string Gerar_N_Conta()
        {
            const string chars = "0123456789";

            Random rng = new Random();

            string N_Conta = string.Empty;

            for (int i = 0; i < 10; i++)
            {
                N_Conta += chars[rng.Next(0, chars.Length)];
            }

            return N_Conta;
        }

    }
}