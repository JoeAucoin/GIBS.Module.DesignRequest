using GIBS.Module.DesignRequest.Migrations.EntityBuilders;
using GIBS.Module.DesignRequest.Repository;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using System.ComponentModel.DataAnnotations;

namespace GIBS.Module.DesignRequest.Server.Migrations
{
    [DbContext(typeof(DesignRequestContext))]
    [Migration("GIBS.Module.DesignRequest.01.00.07.00")]

    public class AddNewColumnsFileToRequest_07 : MultiDatabaseMigration
    {
        public AddNewColumnsFileToRequest_07(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApprovedForClientReview",
                table: "GIBSDesignRequest_FileToRequest",
                nullable: false, // or true
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApproveByClient",
                table: "GIBSDesignRequest_FileToRequest",
                nullable: false, // or true
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "GIBSDesignRequest_FileToRequest", // Name of the table
                type: "nvarchar(max)", // Explicitly set the SQL type
                nullable: true, // Set nullability as needed
                oldClrType: typeof(string),
                oldType: "nvarchar(255)", // Old SQL type/length (example)
                oldMaxLength: 255); // Old MaxLength (example)

            //migrationBuilder.AddColumn<int>(
            //    name: "RevisionsUsed",
            //    table: "GIBSDesignRequest",
            //    nullable: true,
            //    defaultValue: 0);

            //var entityBuilder = new CreditTransactionEntityBuilder(migrationBuilder, ActiveDatabase);
            //entityBuilder.Create();


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedForClientReview",
                table: "GIBSDesignRequest_FileToRequest");

            migrationBuilder.DropColumn(
                name: "ApproveByClient",
                table: "GIBSDesignRequest_FileToRequest");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "GIBSDesignRequest_FileToRequest",
                type: "nvarchar(255)", // Revert to the old SQL type/length
                maxLength: 255, // Set the MaxLength attribute value
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            //migrationBuilder.DropColumn(
            //    name: "RevisionsUsed",
            //   table: "GIBSDesignRequest");

            //var entityBuilder = new CreditTransactionEntityBuilder(migrationBuilder, ActiveDatabase);
            //entityBuilder.Drop();


        }
    }
}