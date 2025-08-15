using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using GIBS.Module.DesignRequest.Repository;

namespace GIBS.Module.DesignRequest.Migrations
{
    [DbContext(typeof(DesignRequestContext))]
    [Migration("DesignRequest.01.00.02.00")]
    public class AddNotes : MultiDatabaseMigration
    {
        public AddNotes(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailNote",
                table: "GIBSDesignRequest_DetailToRequest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
               name: "ApplianceNote",
               table: "GIBSDesignRequest_ApplianceToRequest",
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailNote",
                table: "GIBSDesignRequest_DetailToRequest");

            migrationBuilder.DropColumn(
                name: "ApplianceNote",
               table: "GIBSDesignRequest_ApplianceToRequest");
        }
    }
}