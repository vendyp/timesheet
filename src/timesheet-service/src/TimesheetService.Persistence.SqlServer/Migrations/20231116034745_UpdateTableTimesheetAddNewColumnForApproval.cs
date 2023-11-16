using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimesheetService.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableTimesheetAddNewColumnForApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "Timesheet",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAtServer",
                table: "Timesheet",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedMessage",
                table: "Timesheet",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Timesheet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "Timesheet");

            migrationBuilder.DropColumn(
                name: "ApprovedAtServer",
                table: "Timesheet");

            migrationBuilder.DropColumn(
                name: "RejectedMessage",
                table: "Timesheet");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Timesheet");
        }
    }
}
