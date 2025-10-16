using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOpeningHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpeningHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservableObjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    OpenTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningHours_ReservableObjects_ReservableObjectId",
                        column: x => x.ReservableObjectId,
                        principalTable: "ReservableObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OpeningHours",
                columns: new[] { "Id", "CloseTime", "DayOfWeek", "OpenTime", "ReservableObjectId" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 18, 0, 0, 0), 1, new TimeSpan(0, 8, 0, 0, 0), 20 },
                    { 2, new TimeSpan(0, 18, 0, 0, 0), 2, new TimeSpan(0, 8, 0, 0, 0), 20 },
                    { 3, new TimeSpan(0, 18, 0, 0, 0), 3, new TimeSpan(0, 8, 0, 0, 0), 20 },
                    { 4, new TimeSpan(0, 18, 0, 0, 0), 4, new TimeSpan(0, 8, 0, 0, 0), 20 },
                    { 5, new TimeSpan(0, 18, 0, 0, 0), 5, new TimeSpan(0, 8, 0, 0, 0), 20 },
                    { 6, new TimeSpan(0, 18, 0, 0, 0), 1, new TimeSpan(0, 8, 0, 0, 0), 21 },
                    { 7, new TimeSpan(0, 18, 0, 0, 0), 2, new TimeSpan(0, 8, 0, 0, 0), 21 },
                    { 8, new TimeSpan(0, 18, 0, 0, 0), 3, new TimeSpan(0, 8, 0, 0, 0), 21 },
                    { 9, new TimeSpan(0, 18, 0, 0, 0), 4, new TimeSpan(0, 8, 0, 0, 0), 21 },
                    { 10, new TimeSpan(0, 18, 0, 0, 0), 5, new TimeSpan(0, 8, 0, 0, 0), 21 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpeningHours_ReservableObjectId",
                table: "OpeningHours",
                column: "ReservableObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpeningHours");
        }
    }
}
