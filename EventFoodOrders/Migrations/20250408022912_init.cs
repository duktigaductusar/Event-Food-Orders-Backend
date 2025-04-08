using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventFoodOrders.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    deadline = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    owner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "participants",
                columns: table => new
                {
                    participant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    response_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wants_meal = table.Column<bool>(type: "bit", nullable: false),
                    allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    preferences = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participants", x => x.participant_id);
                    table.ForeignKey(
                        name: "FK_participants_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_participants_event_id",
                table: "participants",
                column: "event_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "participants");

            migrationBuilder.DropTable(
                name: "events");
        }
    }
}
