using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using AbatementHelper.CommonModels.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace AbatementHelper.WebAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        public override string Email { get; set; }
        public string Role { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationUserDbContext : IdentityDbContext
    {
        public ApplicationUserDbContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WebApiUserInfo>().HasOptional(i => i.User).WithMany().HasForeignKey(k => k.UserId);
        }

        public static ApplicationUserDbContext Create()
        {
            return new ApplicationUserDbContext();
        }

        public DbSet<WebApiUserInfo> UserInfo { get; set; }
        public DbSet<WebApiStoreInfo> StoreInfo { get; set; }
        public DbSet<WebApiStoreAdminInfo> StoreAdminInfo { get; set; }
        public DbSet<WebApiAdminInfo> AdminInfo { get; set; }
    }

    //public class ApplicationStore : IdentityUser
    //{
    //    public string PhoneNumber { get; set; }

    //    //public string Country { get; set; }
    //    //public string City { get; set; }
    //    //public string PostalCode { get; set; }
    //    //public string Street { get; set; }

    //    //working hours
    //    public string WorkingHoursWeek { get; set; }
    //    public string WorkingHoursWeekends { get; set; }
    //    public string WorkingHoursHolidays { get; set; }

    //    public string Role { get; set; }

    //    public bool Approved { get; set; }

    //    public string MasterStoreID { get; set; }

    //    public bool Deleted { get; set; }

    //    //public DbSet<Address> Address { get; set; }


    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationStore> manager, string authenticationType)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}



    //public class ApplicationStoreDbContext : IdentityDbContext<ApplicationStore>
    //{
    //    public ApplicationStoreDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationStoreDbContext Create()
    //    {
    //        return new ApplicationStoreDbContext();
    //    }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);
    //        modelBuilder.Entity<ApplicationStore>().ToTable("AspNetStores");
    //        modelBuilder.Entity<IdentityUserRole>().ToTable("AspNetUserRoles");
    //        modelBuilder.Entity<Address>().ToTable("Addresses");
    //    }


    //}

    //public partial class UserAddressEntity : DbContext
    //{
    //    public UserAddressEntity() : base("name=DefaultConnection")
    //    {
    //    }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        /*base.OnModelCreating(modelBuilder);
    //        modelBuilder.Entity<UserAddressEntity>().ToTable("UserAddresses");*/

    //    }

    //    [Key]
    //    public Guid Id { get; set; }
    //    public virtual DbSet<Address> Address { get; set; }
    //}
}