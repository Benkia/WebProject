using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;


namespace WebProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ExternalCookie);

            // Add the claims to user identity, so we can get them from the cshtmls later.
            userIdentity.AddClaim(new Claim("FanID", this.FanID.ToString()));
            userIdentity.AddClaim(new Claim("isAdmin", this.isAdmin.ToString()));

            // Add custom user claims here
            return userIdentity;
        }

        public int FanID { get; set; }
        public bool isAdmin { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            return db;
        }
        public DbSet<Fan> fans { get; set; }

        public System.Data.Entity.DbSet<WebProject.Models.Post> Posts { get; set; }

        public System.Data.Entity.DbSet<WebProject.Models.Comment> Comments { get; set; }

/*        private void createUnknown()
        {
            const int UNKNOWN_ID = -1;

            if (!fans.ToList().Exists(fan => fan.ID == UNKNOWN_ID))
            {
                Fan unknown = new Fan();
                unknown.ID = UNKNOWN_ID;
                unknown.FirstName = "Unknown";
                unknown.LastName = "User";
                unknown.PazamInClub = 0;
                unknown.BirthDay = DateTime.Now;
                unknown.Gender = Gender.Male;
                fans.Add(unknown);

                SaveChanges();
            }
        }
*/
    }
}