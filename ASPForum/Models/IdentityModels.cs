using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASPForum.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Login { get; set; }
        public int Rank { get; set; }
        public string Avatar { get; set; }
        public string Privileges { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

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

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messeges { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Friends> Friends { get; set; }
    }
    public class IdentityManager
    {
        public RoleManager<IdentityRole> LocalRoleManager
        {
            get
            {
                return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            }
        }


        public UserManager<ApplicationUser> LocalUserManager
        {
            get
            {
                return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            }
        }


        public ApplicationUser GetUserByID(string userID)
        {
            ApplicationUser user = null;
            UserManager<ApplicationUser> um = this.LocalUserManager;

            user = um.FindById(userID);

            return user;
        }


        public ApplicationUser GetUserByName(string email)
        {
            ApplicationUser user = null;
            UserManager<ApplicationUser> um = this.LocalUserManager;

            user = um.FindByEmail(email);

            return user;
        }


        public bool RoleExists(string name)
        {
            var rm = LocalRoleManager;

            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = LocalRoleManager;
            var idResult = rm.Create(new IdentityRole(name));

            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = LocalUserManager;
            var idResult = um.Create(user, password);

            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var um = LocalUserManager;
            var idResult = um.AddToRole(userId, roleName);

            return idResult.Succeeded;
        }


        public bool AddUserToRoleByUsername(string username, string roleName)
        {
            var um = LocalUserManager;

            string userID = um.FindByName(username).Id;
            var idResult = um.AddToRole(userID, roleName);

            return idResult.Succeeded;
        }


        public void ClearUserRoles(string userId)
        {
            var um = LocalUserManager;
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);

            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.RoleId);
            }
        }
    }
}