using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QPET.Domain.Entities;
using AdminUser = QPET.Models.AdminUser;

namespace QPET.Data
{
    public class ApplicationDbContext : IdentityDbContext<AdminUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Branch> Branches => Set<Branch>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<SubCategory> SubCategories => Set<SubCategory>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Enquiry> Enquiries => Set<Enquiry>();

        public DbSet<EnquiryStatus> EnquiryStatuses => Set<EnquiryStatus>();

        public DbSet<EnquiryAttachment> EnquiryAttachments =>
            Set<EnquiryAttachment>();

        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Branch>()
                .HasIndex(branch => branch.EmailAddress)
                .IsUnique();

            builder.Entity<Category>()
                .HasIndex(category => category.CategoryName)
                .IsUnique();

            builder.Entity<SubCategory>()
                .HasOne(subCategory => subCategory.Category)
                .WithMany(category => category.SubCategories)
                .HasForeignKey(subCategory => subCategory.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(product => product.SubCategory)
                .WithMany(subCategory => subCategory.Products)
                .HasForeignKey(product => product.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductImage>()
                .HasOne(image => image.Product)
                .WithMany(product => product.Images)
                .HasForeignKey(image => image.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Customer>()
                .HasIndex(customer => customer.EmailAddress)
                .IsUnique();

            builder.Entity<Enquiry>()
                .HasOne(enquiry => enquiry.Customer)
                .WithMany(customer => customer.Enquiries)
                .HasForeignKey(enquiry => enquiry.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Enquiry>()
                .HasOne(enquiry => enquiry.Branch)
                .WithMany(branch => branch.Enquiries)
                .HasForeignKey(enquiry => enquiry.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Enquiry>()
                .HasOne(enquiry => enquiry.EnquiryStatus)
                .WithMany(status => status.Enquiries)
                .HasForeignKey(enquiry => enquiry.EnquiryStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EnquiryAttachment>()
                .HasOne(attachment => attachment.Enquiry)
                .WithMany(enquiry => enquiry.Attachments)
                .HasForeignKey(attachment => attachment.EnquiryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(review => review.Branch)
                .WithMany(branch => branch.Reviews)
                .HasForeignKey(review => review.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(review => review.Customer)
                .WithMany(customer => customer.Reviews)
                .HasForeignKey(review => review.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<EnquiryStatus>()
                .HasIndex(status => status.StatusName)
                .IsUnique();
        }
    }
}