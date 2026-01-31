using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Is.Repository.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseInOrder_Courses_CourseId",
                table: "CourseInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseInOrder_Order_OrderId",
                table: "CourseInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseInMyCoursesCards",
                table: "CourseInMyCoursesCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseInOrder",
                table: "CourseInOrder");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "CourseInOrder",
                newName: "CourseInOrders");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseInOrder_OrderId",
                table: "CourseInOrders",
                newName: "IX_CourseInOrders_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CourseInOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseInMyCoursesCards",
                table: "CourseInMyCoursesCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseInOrders",
                table: "CourseInOrders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MailTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseInMyCoursesCards_CourseId",
                table: "CourseInMyCoursesCards",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseInOrders_CourseId",
                table: "CourseInOrders",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInOrders_Courses_CourseId",
                table: "CourseInOrders",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInOrders_Orders_OrderId",
                table: "CourseInOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseInOrders_Courses_CourseId",
                table: "CourseInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseInOrders_Orders_OrderId",
                table: "CourseInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "EmailMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseInMyCoursesCards",
                table: "CourseInMyCoursesCards");

            migrationBuilder.DropIndex(
                name: "IX_CourseInMyCoursesCards_CourseId",
                table: "CourseInMyCoursesCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseInOrders",
                table: "CourseInOrders");

            migrationBuilder.DropIndex(
                name: "IX_CourseInOrders_CourseId",
                table: "CourseInOrders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CourseInOrders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "CourseInOrders",
                newName: "CourseInOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Order",
                newName: "IX_Order_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseInOrders_OrderId",
                table: "CourseInOrder",
                newName: "IX_CourseInOrder_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseInMyCoursesCards",
                table: "CourseInMyCoursesCards",
                columns: new[] { "CourseId", "MyCourseCardId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseInOrder",
                table: "CourseInOrder",
                columns: new[] { "CourseId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInOrder_Courses_CourseId",
                table: "CourseInOrder",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInOrder_Order_OrderId",
                table: "CourseInOrder",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
