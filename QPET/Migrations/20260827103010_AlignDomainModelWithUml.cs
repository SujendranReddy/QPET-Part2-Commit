using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QPET.Migrations
{
    /// <inheritdoc />
    public partial class AlignDomainModelWithUml : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiries_EnquiryStatuses_EnquiryStatusId",
                table: "Enquiries");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Customers_CustomerId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "EnquiryStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CustomerId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Enquiries_EnquiryStatusId",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "EnquiryAttachments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reviews",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "EnquiryAttachments",
                newName: "UploadedDate");

            migrationBuilder.RenameColumn(
                name: "EnquiryAttachmentId",
                table: "EnquiryAttachments",
                newName: "AttachmentId");

            migrationBuilder.RenameColumn(
                name: "EnquiryStatusId",
                table: "Enquiries",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Enquiries",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<string>(
                name: "SubCategoryDescription",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CategoryDescription",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleMapLink",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1,
                columns: new[] { "GoogleMapLink", "PostalCode", "StreetAddress" },
                values: new object[] { null, "", "" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2,
                columns: new[] { "GoogleMapLink", "PostalCode", "StreetAddress" },
                values: new object[] { null, "", "" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 3,
                columns: new[] { "GoogleMapLink", "PostalCode", "StreetAddress" },
                values: new object[] { null, "", "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CategoryDescription",
                value: "PET bottle packaging solutions.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryDescription",
                value: "PET jar packaging solutions.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CategoryDescription",
                value: "PET preforms for packaging manufacturing.");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 1,
                column: "SubCategoryDescription",
                value: null);

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 2,
                column: "SubCategoryDescription",
                value: null);

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 3,
                column: "SubCategoryDescription",
                value: null);

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 4,
                column: "SubCategoryDescription",
                value: null);

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 5,
                column: "SubCategoryDescription",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubCategoryDescription",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryDescription",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "GoogleMapLink",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Reviews",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UploadedDate",
                table: "EnquiryAttachments",
                newName: "UploadedAt");

            migrationBuilder.RenameColumn(
                name: "AttachmentId",
                table: "EnquiryAttachments",
                newName: "EnquiryAttachmentId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Enquiries",
                newName: "EnquiryStatusId");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Enquiries",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "EnquiryAttachments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Enquiries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EnquiryStatuses",
                columns: table => new
                {
                    EnquiryStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryStatuses", x => x.EnquiryStatusId);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "Description",
                value: "PET bottle packaging solutions.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Description",
                value: "PET jar packaging solutions.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "Description",
                value: "PET preforms for packaging manufacturing.");

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

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerId",
                table: "Reviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_EnquiryStatusId",
                table: "Enquiries",
                column: "EnquiryStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryStatuses_StatusName",
                table: "EnquiryStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiries_EnquiryStatuses_EnquiryStatusId",
                table: "Enquiries",
                column: "EnquiryStatusId",
                principalTable: "EnquiryStatuses",
                principalColumn: "EnquiryStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Customers_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
