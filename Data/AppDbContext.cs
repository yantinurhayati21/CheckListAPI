using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckListAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckListAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<CheckList> CheckLists { get; set; }
        public DbSet<CheckListItem> CheckListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>().Property(u => u.UpdatedAt).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<CheckList>().Property(vb => vb.CreatedAt).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<CheckList>().Property(vb => vb.UpdatedAt).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<CheckListItem>().Property(vt => vt.CreatedAt).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<CheckListItem>().Property(vt => vt.UpdatedAt).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<CheckListItem>().Property(v => v.Status)
                .HasConversion<string>();

            modelBuilder.Entity<CheckListItem>()
               .HasOne(vt => vt.CheckList)
               .WithMany()
               .HasForeignKey(vt => vt.CheckListId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}