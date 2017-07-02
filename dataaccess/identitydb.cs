namespace dataaccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    public partial class identitydb : IdentityDbContext<User>
    {
        public identitydb()
            : base("name=identitydb")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
        }
        //public static identitydb Create()
        //{
        //    return new identitydb();
        //}
    }
}
