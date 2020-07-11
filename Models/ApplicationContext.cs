using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProj> UserProjs { get; set; }

        public DbSet<Project> Projects { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string projDirect = "projDirect";
            string staff = "staff";

            string projDirectEmail = "hiimsenya@gmail.com";
            string projDirectPassword = "Qwe123!@#";

            Role projDirectRole = new Role { Id = 1, Name = projDirect };
            Role staffRole = new Role { Id = 2, Name = staff };
            User projDirectUser = new User { UserId = 1, Email = projDirectEmail, Password = projDirectPassword, RoleId = projDirectRole.Id, Name = "Арсений" };
           
            modelBuilder.Entity<Role>().HasData(new Role[] { projDirectRole, staffRole });
            modelBuilder.Entity<User>().HasData(new User[] { projDirectUser });


            modelBuilder.Entity<UserProj>().HasKey(t => new { t.UserId, t.ProjectId });

            modelBuilder.Entity<UserProj>().HasOne(pt => pt.User).WithMany(p => p.UserProjs).HasForeignKey(pt => pt.UserId);
            modelBuilder.Entity<UserProj>().HasOne(pt => pt.Projects).WithMany(p => p.UserProjs).HasForeignKey(pt => pt.ProjectId);

        }
    }
}
