using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using GIBS.Module.DesignRequest.Repository;

namespace GIBS.Module.DesignRequest.Migrations
{
    [DbContext(typeof(DesignRequestContext))]
    [Migration("DesignRequest.01.00.01.00")]
    public class AddUserId : MultiDatabaseMigration
    {
        public AddUserId(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "GIBSDesignRequest",
                nullable: true, 
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
               name: "MailingAddress",
               table: "GIBSDesignRequest",
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GIBSDesignRequest");

            migrationBuilder.DropColumn(
                name: "MailingAddress",
               table: "GIBSDesignRequest");
        }
    }
}