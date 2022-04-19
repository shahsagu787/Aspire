using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspire.Migrations.AppDB
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostStatuses",
                columns: table => new
                {
                    PostStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descriptiion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostStatuses", x => x.PostStatusId);
                });

            migrationBuilder.CreateTable(
                name: "PostTypes",
                columns: table => new
                {
                    PostTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descriptiion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTypes", x => x.PostTypeId);
                });

            migrationBuilder.CreateTable(
                name: "UserPostSuport",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPostSuport", x => new { x.PostId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Support = table.Column<int>(type: "int", nullable: false),
                    PostedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostStatusId = table.Column<int>(type: "int", nullable: false),
                    PostTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_PostStatuses_PostStatusId",
                        column: x => x.PostStatusId,
                        principalTable: "PostStatuses",
                        principalColumn: "PostStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_PostTypes_PostTypeId",
                        column: x => x.PostTypeId,
                        principalTable: "PostTypes",
                        principalColumn: "PostTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PostStatuses",
                columns: new[] { "PostStatusId", "CreatedOn", "Descriptiion", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "This status shows the post is open to use with Auther's permission.", "Open" },
                    { 2, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "This status shows that the post is adopted and under cunstruction.", "In Progress" },
                    { 3, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "This status shows that the post is completed but still open to adopt with auther's permission.", "Completed" },
                    { 4, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "This status shows that the post should not be displayed to anyone.", "Private" },
                    { 5, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "This post is forcefully removed from the post library.", "Blocked" }
                });

            migrationBuilder.InsertData(
                table: "PostTypes",
                columns: new[] { "PostTypeId", "CreatedOn", "Descriptiion", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "The post that use this type is a new and has to be builded from the scratch.", "New" },
                    { 2, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "The post that use this type is something already exists and has to be updated it could be anything including adding a new feature.", "Update" },
                    { 3, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "The post that use this type is something that already exists and Buged that need to be Fixed.", "Bug" },
                    { 4, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "The post that use this type is Something that only required Design.", "Design" },
                    { 5, new DateTime(2022, 4, 13, 0, 0, 0, 0, DateTimeKind.Local), "The post that use this type is Something that is designed and need cunstruction", "Cunstruction" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostStatusId",
                table: "Posts",
                column: "PostStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostTypeId",
                table: "Posts",
                column: "PostTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "UserPostSuport");

            migrationBuilder.DropTable(
                name: "PostStatuses");

            migrationBuilder.DropTable(
                name: "PostTypes");
        }
    }
}
