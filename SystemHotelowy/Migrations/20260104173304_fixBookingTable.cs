using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemHotelowy.Migrations
{
    /// <inheritdoc />
    public partial class fixBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_AppUserId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Bookings",
                newName: "ReceptionistId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_AppUserId",
                table: "Bookings",
                newName: "IX_Bookings_ReceptionistId");

            migrationBuilder.AlterColumn<string>(
                name: "VisitorId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "af7609d0-341c-4f1b-a044-50c4d7f7501c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "70ce88ac-de07-4bc3-9ba4-12ec8823befb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "e9946e30-879e-4220-8b99-d314895eeb91");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VisitorId",
                table: "Bookings",
                column: "VisitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_ReceptionistId",
                table: "Bookings",
                column: "ReceptionistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_VisitorId",
                table: "Bookings",
                column: "VisitorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_ReceptionistId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_VisitorId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_VisitorId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ReservationDate",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ReceptionistId",
                table: "Bookings",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ReceptionistId",
                table: "Bookings",
                newName: "IX_Bookings_AppUserId");

            migrationBuilder.AlterColumn<string>(
                name: "VisitorId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "d74d74dd-60e8-413a-9a5f-b2d22e13f644");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "c91c4027-b869-4d08-afb9-2e8b7e7f55be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "c68441a8-e84b-40d3-9d5d-76a74fb3ef2b");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_AppUserId",
                table: "Bookings",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
