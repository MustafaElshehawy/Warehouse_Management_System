using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<PurchasesHeader>()
                .HasOne(p => p.Warehouse)
                .WithMany()
                .HasForeignKey(p => p.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchasesHeader>()
                .HasOne(p => p.Branche)  
                .WithMany()
                .HasForeignKey(p => p.BrancheId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PurchasesDetails>()
                .HasOne(pd => pd.Purchase)
                .WithMany(ph => ph.Items)
                .HasForeignKey(pd => pd.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PurchasesReturnHeader>()
                .HasOne(ph => ph.PurchasesHeader)
                .WithMany() 
                .HasForeignKey(ph => ph.PurchaseHeaderId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<PurchasesReturnDetails>()
                .HasOne(pd => pd.PurchasesReturnHeader)
                .WithMany(ph => ph.Items)
                .HasForeignKey(pd => pd.PurchasesReturnHeaderId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<SaleDetails>(entity => { 

                entity.HasOne(sd => sd.SaleHeader)
                 .WithMany(sd => sd.Items)
                 .HasForeignKey(sd => sd.SaleHeaderId)
                 .OnDelete(DeleteBehavior.Cascade);//علشان امسح التفاصيل لو مسحت الفاتوره 

                entity.HasOne(sd => sd.Product)
                    .WithMany()
                    .HasForeignKey(sd => sd.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);//منعت  احذف المنتح لو له فاتوره 
            });

            modelBuilder.Entity<SaleHeader>(entity => {

                entity.HasOne(sh => sh.Branche)
                .WithMany()
                .HasForeignKey(sh => sh.BrancheId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sh => sh.Warehouse)
                .WithMany()
                .HasForeignKey(sh => sh.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
            
            });

                 
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductUnit> ProductUnits { get; set; }

        public DbSet<Branche> Branches { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockMovement> StockMovements { get; set; }

        public DbSet<PurchasesHeader> PurchasesHeaders { get; set; }

        public DbSet<PurchasesDetails> PurchasesDetails { get; set; }

        public DbSet<PurchasesReturnHeader> PurchasesReturnHeaders { get; set; }

        public DbSet<PurchasesReturnDetails> PurchasesReturnDetails { get; set; }

        public DbSet<SaleHeader> SaleHeaders { get; set; }
        public DbSet<SaleDetails> SaleDetails { get; set; }

    }

}
