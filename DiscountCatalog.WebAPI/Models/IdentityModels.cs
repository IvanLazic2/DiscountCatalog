using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DiscountCatalog.WebAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        [Index(IsUnique = true)]
        [EmailAddress]
        public override string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        public override string UserName { get; set; }
        public ImageEntity UserImage { get; set; }

        //public byte[] UserImage { get; set; }

        public ApplicationUser()
        {
            
        }

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
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ManagerEntity>()
            //    .HasRequired(m => m.Identity)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<ManagerEntity>()
            //    .HasRequired(m => m.Administrator)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ManagerEntity>()
            //    .HasOptional(m => m.Stores)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public static ApplicationUserDbContext Create()
        {
            return new ApplicationUserDbContext();
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<StoreAdminEntity> StoreAdmins { get; set; }
        public DbSet<StoreEntity> Stores { get; set; }
        public DbSet<ManagerEntity> Managers { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
    }

    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager(UserStore<ApplicationUser> userStore)
            : base(userStore)
        {
        }
    }

    public class RoleManager : RoleManager<IdentityRole>
    {
        public RoleManager(RoleStore<IdentityRole> roleStore)
            :base(roleStore)
        {
            RoleStore<IdentityRole> r = new RoleStore<IdentityRole>();
            
        }
    }
}