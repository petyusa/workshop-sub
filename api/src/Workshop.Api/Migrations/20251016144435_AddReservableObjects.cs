using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddReservableObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservableObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservableObjects_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ReservableObjects",
                columns: new[] { "Id", "Description", "IsActive", "LocationId", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Window seat with natural light", true, 1, "Desk A-101", "Desk" },
                    { 2, "Standing desk available", true, 1, "Desk A-102", "Desk" },
                    { 3, "Quiet corner desk", true, 1, "Desk A-103", "Desk" },
                    { 4, "Under maintenance", false, 1, "Desk A-104", "Desk" },
                    { 5, "Near collaboration area", true, 1, "Desk A-105", "Desk" },
                    { 6, "Covered parking", true, 1, "Parking DT-01", "ParkingSpace" },
                    { 7, "Ground level", true, 1, "Parking DT-02", "ParkingSpace" },
                    { 8, "EV charging available", true, 1, "Parking DT-03", "ParkingSpace" },
                    { 9, "Ergonomic chair included", true, 2, "Desk B-201", "Desk" },
                    { 10, "Dual monitor setup", true, 2, "Desk B-202", "Desk" },
                    { 11, "Private booth desk", true, 2, "Desk B-203", "Desk" },
                    { 12, "Open space desk", true, 2, "Desk B-204", "Desk" },
                    { 13, "Outdoor parking", true, 2, "Parking NB-01", "ParkingSpace" },
                    { 14, "Covered parking", true, 2, "Parking NB-02", "ParkingSpace" },
                    { 15, "Reserved for maintenance vehicle", false, 2, "Parking NB-03", "ParkingSpace" },
                    { 16, "Corner office desk", true, 3, "Desk C-301", "Desk" },
                    { 17, "Collaborative workspace", true, 3, "Desk C-302", "Desk" },
                    { 18, "Focus room desk", true, 3, "Desk C-303", "Desk" },
                    { 19, "Underground parking", true, 3, "Parking ES-01", "ParkingSpace" },
                    { 20, "8-person capacity, video conferencing", true, 3, "Meeting Room Alpha", "MeetingRoom" },
                    { 21, "4-person capacity, whiteboard", true, 3, "Meeting Room Beta", "MeetingRoom" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservableObjects_LocationId",
                table: "ReservableObjects",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservableObjects");
        }
    }
}
