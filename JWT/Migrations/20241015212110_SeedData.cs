using Microsoft.EntityFrameworkCore.Migrations;

namespace JWT.Migrations;

public partial class SeedData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
            values: new object[] { Guid.NewGuid().ToString(), "User", "USER", Guid.NewGuid().ToString() }
        );

        migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
            values: new object[] { Guid.NewGuid().ToString(), "Admin", "ADMIN", Guid.NewGuid().ToString() }
        );

    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Name",
            keyValues: new object[] { "User", "Admin" }
        );
    }
}
