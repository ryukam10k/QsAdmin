namespace QsAdmin.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QsAdmin.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(QsAdmin.Models.ApplicationDbContext context)
        {
            var aspNetUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "0000",
                    AccountName = "Admin",
                    Email = "dummy@qsworks.net",
                    PasswordHash = "password",
                    UserName = "dummy@qsworks.net",
                },
            };

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = "1",
                    Name = "Admin"
                }
            };

            var userRoles = new List<IdentityUserRole>
            {
                new IdentityUserRole()
                {
                    RoleId = "1",
                    UserId = "0000"
                }
            };

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            foreach (ApplicationUser data in aspNetUsers)
            {
                if (userManager.FindById(data.Id) == null)
                {
                    userManager.Create(data, data.PasswordHash);

                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    roles.ForEach(r => roleManager.Create(r));
                    userManager.AddToRole("0000", "Admin");
                    context.SaveChanges();
                }
            }

            context.SaveChanges();
        }
    }
}
