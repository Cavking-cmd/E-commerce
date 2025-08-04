using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Migrations
{
    /// <inheritdoc />
    public partial class SECOND : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewDate",
                table: "Reviews",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageFile",
                table: "Products",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Products",
                type: "longtext",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("3c91647c-3681-4912-b666-9fd5a31a8b09"), new DateTime(2025, 8, 3, 3, 31, 50, 174, DateTimeKind.Utc).AddTicks(4455), null, false, "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "IsDeleted", "Password" },
                values: new object[] { new Guid("32a95c71-7631-45e7-aab2-694498a2fb9d"), new DateTime(2025, 8, 3, 3, 31, 50, 174, DateTimeKind.Utc).AddTicks(4942), "admin@yopmail.com", false, "$2a$11$KEKSK.ruefTgSYMEyPOfh.MG7zn2ADIcVym9SuRr/VLPCGMrEkJOO" });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "AddressLine", "City", "Country", "CreatedDate", "Email", "FirstName", "IsDeleted", "LastName", "PhoneNumber", "PostalCode", "State", "UserId" },
                values: new object[] { new Guid("b9dbfaea-28e3-46bb-a4dc-8d3ba266dab8"), "123 Admin Street", "Admin City", "Adminland", new DateTime(2025, 8, 3, 3, 31, 50, 174, DateTimeKind.Utc).AddTicks(5022), "admin1@yopmail.com", "Admin", false, "User", "08000000000", "100001", "Admin State", new Guid("32a95c71-7631-45e7-aab2-694498a2fb9d") });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "RoleId", "UserId" },
                values: new object[] { new Guid("cb0b7dfe-1ea4-4694-abb9-6e1143fb1487"), new DateTime(2025, 8, 3, 3, 31, 50, 174, DateTimeKind.Utc).AddTicks(5117), false, new Guid("3c91647c-3681-4912-b666-9fd5a31a8b09"), new Guid("32a95c71-7631-45e7-aab2-694498a2fb9d") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: new Guid("b9dbfaea-28e3-46bb-a4dc-8d3ba266dab8"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("cb0b7dfe-1ea4-4694-abb9-6e1143fb1487"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3c91647c-3681-4912-b666-9fd5a31a8b09"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("32a95c71-7631-45e7-aab2-694498a2fb9d"));

            migrationBuilder.DropColumn(
                name: "ImageFile",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Products");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReviewDate",
                table: "Reviews",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "longtext",
                nullable: false);
        }
    }
}
