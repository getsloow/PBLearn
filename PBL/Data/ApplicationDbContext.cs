using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PBL.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace PBL.Data
{


    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<AssignmentModel> Assignments { get; set; }

        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<Models.FileModel> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AssignmentModel>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Assignments)
                .HasForeignKey(a => a.ProjectId);
        }
    }

}
