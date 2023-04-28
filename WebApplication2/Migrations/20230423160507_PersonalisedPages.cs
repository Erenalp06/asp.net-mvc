using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class PersonalisedPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "PPagess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CVText = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CVPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioText = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PortfolioPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AboutMe = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PageImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PageURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPagess", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "PPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutMe = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CVPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CVText = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PageImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioText = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPages", x => x.Id);
                });
        }
    }
}
