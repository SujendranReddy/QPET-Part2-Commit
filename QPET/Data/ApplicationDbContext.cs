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
               .HasOne(product => product.Category)
               .WithMany(category => category.Products)
               .HasForeignKey(product => product.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(product => product.SubCategory)
                .WithMany(subCategory => subCategory.Products)
                .HasForeignKey(product => product.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductImage>()
                .HasOne(image => image.Product)
                .WithMany(product => product.ProductImages)
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


            builder.Entity<EnquiryAttachment>()
                .HasKey(attachment => attachment.AttachmentId);

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

            builder.Entity<Branch>().HasData(
    new Branch
    {
        BranchId = 1,
        BranchName = "Pietermaritzburg",
        PhoneNumber = "000 000 0000",
        EmailAddress = "pietermaritzburg@qpet.co.za",
        City = "Pietermaritzburg",
        Province = "KwaZulu-Natal"
    },
    new Branch
    {
        BranchId = 2,
        BranchName = "Johannesburg",
        PhoneNumber = "000 000 0000",
        EmailAddress = "johannesburg@qpet.co.za",
        City = "Johannesburg",
        Province = "Gauteng"
    },
    new Branch
    {
        BranchId = 3,
        BranchName = "Cape Town",
        PhoneNumber = "000 000 0000",
        EmailAddress = "capetown@qpet.co.za",
        City = "Cape Town",
        Province = "Western Cape"
    });

            builder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Bottles",
                    CategoryDescription = "PET bottle packaging solutions."
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Jars",
                    CategoryDescription = "PET jar packaging solutions."
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Preforms",
                    CategoryDescription = "PET preforms for packaging manufacturing."
                });

            builder.Entity<SubCategory>().HasData(
                new SubCategory
                {
                    SubCategoryId = 1,
                    CategoryId = 1,
                    SubCategoryName = "Water Bottles"
                },
                new SubCategory
                {
                    SubCategoryId = 2,
                    CategoryId = 1,
                    SubCategoryName = "Juice Bottles"
                },
                new SubCategory
                {
                    SubCategoryId = 3,
                    CategoryId = 1,
                    SubCategoryName = "Beverage Bottles"
                },
                new SubCategory
                {
                    SubCategoryId = 4,
                    CategoryId = 2,
                    SubCategoryName = "Food Jars"
                },
                new SubCategory
                {
                    SubCategoryId = 5,
                    CategoryId = 3,
                    SubCategoryName = "Standard Preforms"
                });
        }
    }
}