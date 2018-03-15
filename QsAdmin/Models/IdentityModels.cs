using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;

namespace QsAdmin.Models
{
    // ApplicationUser クラスにプロパティを追加することでユーザーのプロファイル データを追加できます。詳細については、http://go.microsoft.com/fwlink/?LinkID=317594 を参照してください。
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("アカウント名")]
        public string AccountName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType が CookieAuthenticationOptions.AuthenticationType で定義されているものと一致している必要があります
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // ここにカスタム ユーザー クレームを追加します
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<QsAdmin.Models.Deal> Deals { get; set; }

        public System.Data.Entity.DbSet<QsAdmin.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<QsAdmin.Models.DealCategory> DealCategories { get; set; }

        public System.Data.Entity.DbSet<QsAdmin.Models.ToDo> ToDos { get; set; }

        public System.Data.Entity.DbSet<QsAdmin.Models.WebLink> WebLinks { get; set; }

    }
}