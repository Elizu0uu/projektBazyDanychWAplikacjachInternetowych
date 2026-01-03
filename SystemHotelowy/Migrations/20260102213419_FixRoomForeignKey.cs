using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemHotelowy.Migrations
{
    /// <inheritdoc />
    public partial class FixRoomForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomsId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomsId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomsId",
                table: "Bookings");

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

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "RoomsId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "75201264-b9f9-430c-a6e7-16aafca8f377");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "ca343bea-5937-4b82-b315-9e6df3a73583");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "301fca74-f81d-47ac-989e-ec1f588ae94a");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomsId",
                table: "Bookings",
                column: "RoomsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomsId",
                table: "Bookings",
                column: "RoomsId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
