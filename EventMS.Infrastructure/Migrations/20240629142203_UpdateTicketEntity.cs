using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketNumber",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tickets",
                newName: "TypeTicketId");

            migrationBuilder.CreateTable(
                name: "TypeTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeTickets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TypeTicketId",
                table: "Tickets",
                column: "TypeTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TypeTickets_TypeTicketId",
                table: "Tickets",
                column: "TypeTicketId",
                principalTable: "TypeTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TypeTickets_TypeTicketId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TypeTickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TypeTicketId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TypeTicketId",
                table: "Tickets",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "TicketNumber",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
