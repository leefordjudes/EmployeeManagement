﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagement.Migrations
{
    public partial class SeedEmployeesTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "Id", "Department", "Email", "Name" },
                values: new object[] { 1, 2, "mark@pragimtech.com", "Mark" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
