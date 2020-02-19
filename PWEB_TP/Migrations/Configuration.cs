namespace PWEB_TP.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PWEB_TP.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PWEB_TP.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        String[] roles = { "Administrador", "Aluno", "ComissaoEstagios", "Docente", "Empresa" };

        protected override void Seed(PWEB_TP.Models.ApplicationDbContext context)
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            for (int i = 0; i < roles.Length; i++)
            {
                if (RoleManager.RoleExists(roles[i]) == false)
                {
                    RoleManager.Create(new IdentityRole(roles[i]));
                }
            }
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var PasswordHash = new PasswordHasher();
            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    PasswordHash = PasswordHash.HashPassword("123456")
                };

                UserManager.Create(user);
                UserManager.AddToRole(user.Id, roles[0]);
            }
            if (!context.Users.Any(u => u.UserName == "aluno@aluno.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "aluno@aluno.com",
                    Email = "aluno@aluno.com",
                    PasswordHash = PasswordHash.HashPassword("123456")
                };

                UserManager.Create(user);
                UserManager.AddToRole(user.Id, roles[1]);
            }
            if (!context.Users.Any(u => u.UserName == "docente@docente.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "docente@docente.com",
                    Email = "docente@docente.com",
                    PasswordHash = PasswordHash.HashPassword("123456")
                };

                UserManager.Create(user);
                UserManager.AddToRole(user.Id, roles[3]);
            }
            if (!context.Users.Any(u => u.UserName == "empresa@empresa.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "empresa@empresa.com",
                    Email = "empresa@empresa.com",
                    PasswordHash = PasswordHash.HashPassword("123456")
                };

                UserManager.Create(user);
                UserManager.AddToRole(user.Id, roles[4]);
            }
        }
    }
}
