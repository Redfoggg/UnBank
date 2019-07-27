using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UnBank.models 
{
    public class AuthenticationContext: IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUser {get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //AspNetUsers -> N_Conta mudança de nome da tabela padrão do Identity, link https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-2.2
            builder.Entity<IdentityUser>(b =>
            {
                b.Property(e => e.UserName).HasColumnName("Cpf");
            });
        }
    }
}