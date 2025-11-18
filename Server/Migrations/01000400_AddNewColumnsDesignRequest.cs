using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using GIBS.Module.DesignRequest.Repository;

namespace GIBS.Module.DesignRequest.Server.Migrations
{
    [DbContext(typeof(DesignRequestContext))]
    [Migration("GIBS.Module.DesignRequest.01.00.04.00")]
    
    public class AddNewColumnsDesignRequest : MultiDatabaseMigration
    {
        public AddNewColumnsDesignRequest(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectManagerUserId",
                table: "GIBSDesignRequest",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
               name: "ProjectStatus",
               table: "GIBSDesignRequest", maxLength: 100,
               nullable: true, defaultValue: "Active");

            migrationBuilder.AddColumn<string>(
               name: "HandlePull", 
               table: "GIBSDesignRequest", maxLength: 100,
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectManagerUserId",
                table: "GIBSDesignRequest");

            migrationBuilder.DropColumn(
                name: "ProjectStatus",
               table: "GIBSDesignRequest");

            migrationBuilder.DropColumn(
               name: "HandlePull",
               table: "GIBSDesignRequest");
        }
    }
}