using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LeaveRequests",
                columns: new[] { "Id", "CreatedAt", "EmployeeId", "EndDate", "ReviewerNote", "StartDate", "Status", "Type" },
                values: new object[,]
                {
                    { 4, new DateTimeOffset(new DateTime(2026, 4, 13, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateOnly(2026, 4, 13), "No notice given and no certificate provided.", new DateOnly(2026, 4, 13), "Rejected", "Sick" },
                    { 5, new DateTimeOffset(new DateTime(2026, 4, 2, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5, new DateOnly(2026, 5, 15), null, new DateOnly(2026, 5, 4), "Pending", "Unpaid" },
                    { 6, new DateTimeOffset(new DateTime(2026, 4, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 6, new DateOnly(2026, 5, 20), null, new DateOnly(2026, 5, 18), "Pending", "Vacation" },
                    { 7, new DateTimeOffset(new DateTime(2026, 6, 1, 6, 50, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 7, new DateOnly(2026, 6, 5), "Approved; workload reassigned.", new DateOnly(2026, 6, 1), "Approved", "Sick" },
                    { 8, new DateTimeOffset(new DateTime(2026, 5, 30, 16, 20, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 8, new DateOnly(2026, 6, 26), "Peak delivery week; please reschedule.", new DateOnly(2026, 6, 22), "Rejected", "Vacation" },
                    { 9, new DateTimeOffset(new DateTime(2026, 6, 10, 9, 45, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 9, new DateOnly(2026, 7, 17), null, new DateOnly(2026, 7, 6), "Pending", "Vacation" },
                    { 10, new DateTimeOffset(new DateTime(2026, 7, 1, 13, 10, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 10, new DateOnly(2026, 7, 21), null, new DateOnly(2026, 7, 20), "Pending", "Unpaid" },
                    { 11, new DateTimeOffset(new DateTime(2026, 2, 10, 9, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateOnly(2026, 3, 6), "Approved; cover arranged with the team.", new DateOnly(2026, 3, 2), "Approved", "Vacation" },
                    { 12, new DateTimeOffset(new DateTime(2026, 3, 16, 7, 40, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateOnly(2026, 3, 18), "Medical certificate received.", new DateOnly(2026, 3, 16), "Approved", "Sick" },
                    { 13, new DateTimeOffset(new DateTime(2026, 3, 20, 11, 5, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateOnly(2026, 4, 10), null, new DateOnly(2026, 4, 6), "Pending", "Vacation" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
