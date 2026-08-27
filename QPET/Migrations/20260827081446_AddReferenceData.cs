using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QPET.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "BranchName", "City", "EmailAddress", "PhoneNumber", "Province" },
                values: new object[,]
                {
                    { 1, "Pietermaritzburg", "Pietermaritzburg", "pietermaritzburg@qpet.co.za", "000 000 0000", "KwaZulu-Natal" },
                    { 2, "Johannesburg", "Johannesburg", "johannesburg@qpet.co.za", "000 000 0000", "Gauteng" },
                    { 3, "Cape Town", "Cape Town", "capetown@qpet.co.za", "000 000 0000", "Western Cape" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Bottles", "PET bottle packaging solutions." },
                    { 2, "Jars", "PET jar packaging solutions." },
                    { 3, "Preforms", "PET preforms for packaging manufacturing." }
                });

            migrationBuilder.InsertData(
                table: "EnquiryStatuses",
                columns: new[] { "EnquiryStatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "New" },
                    { 2, "In Progress" },
                    { 3, "Resolved" },
                    { 4, "Closed" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "SubCategoryId", "CategoryId", "SubCategoryName" },
                values: new object[,]
                {
                    { 1, 1, "Water Bottles" },
                    { 2, 1, "Juice Bottles" },
                    { 3, 1, "Beverage Bottles" },
                    { 4, 2, "Food Jars" },
                    { 5, 3, "Standard Preforms" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EnquiryStatuses",
                keyColumn: "EnquiryStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EnquiryStatuses",
                keyColumn: "EnquiryStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EnquiryStatuses",
                keyColumn: "EnquiryStatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EnquiryStatuses",
                keyColumn: "EnquiryStatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);
        }
    }
}
