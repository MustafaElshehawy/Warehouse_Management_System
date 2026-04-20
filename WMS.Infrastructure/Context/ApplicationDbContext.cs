using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductUnit>()
                .HasOne(pu => pu.ChildUnit)
                .WithMany()
                .HasForeignKey(pu => pu.ChildUnitId)
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<ProductUnit>()
                .HasOne(pu => pu.ParentUnit)
                .WithMany()
                .HasForeignKey(pu => pu.ParentUnitId)
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<Product>()
                .HasOne(p => p.Unit)
                .WithMany()
                .HasForeignKey(p => p.SmallestUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Stock>()
                .HasIndex(s => new { s.WarehouseId, s.ProductId, s.UnitId }).IsUnique();//ليه مش HasKey ===> لان لو انت ربط  هتاخد 3 اعمده ودا مش حلو   --(aالحل استخدام index ضمنت المخزن بتاعك مفهوش تكرارa) 
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Product>Products { get; set; }

        public DbSet<ProductUnit> ProductUnits { get; set; }

        public DbSet<Branche> Branches { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockMovement> StockMovements { get; set; }

    }

}
