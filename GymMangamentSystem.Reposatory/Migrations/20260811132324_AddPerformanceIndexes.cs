using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymMangamentSystem.Reposatory.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BMIRecords_UserId",
                table: "BMIRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MeasurementDate",
                table: "BMIRecords",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BMIRecords_UserId_MeasurementDate",
                table: "BMIRecords",
                columns: new[] { "UserId", "MeasurementDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_UserCode",
                table: "Attendances",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserCode",
                table: "AspNetUsers",
                column: "UserCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BMIRecords_UserId_MeasurementDate",
                table: "BMIRecords");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_UserCode",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MeasurementDate",
                table: "BMIRecords",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_BMIRecords_UserId",
                table: "BMIRecords",
                column: "UserId");
        }
    }
}
