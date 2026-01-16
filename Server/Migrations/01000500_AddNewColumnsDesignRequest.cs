using GIBS.Module.DesignRequest.Migrations.EntityBuilders;
using GIBS.Module.DesignRequest.Repository;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;

namespace GIBS.Module.DesignRequest.Server.Migrations
{
    [DbContext(typeof(DesignRequestContext))]
    [Migration("GIBS.Module.DesignRequest.01.00.05.00")]

    public class AddNewColumnsDesignRequest_05 : MultiDatabaseMigration
    {
        public AddNewColumnsDesignRequest_05(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditsSpent",
                table: "GIBSDesignRequest",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RevisionsUsed",
                table: "GIBSDesignRequest",
                nullable: true,
                defaultValue: 0);

            var entityBuilder = new CreditTransactionEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Create();

            var entityBuilder1 = new PaymentRecordEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder1.Create();

            var entityBuilder2 = new CreditPackageEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder2.Create();

            //UserCreditEntityBuilder
            var entityBuilder3 = new UserCreditEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder3.Create();


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditsSpent",
                table: "GIBSDesignRequest");

            migrationBuilder.DropColumn(
                name: "RevisionsUsed",
               table: "GIBSDesignRequest");

            var entityBuilder = new CreditTransactionEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Drop();

            var entityBuilder1 = new PaymentRecordEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder1.Create();

            var entityBuilder2 = new CreditPackageEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder2.Create();

            var entityBuilder3 = new UserCreditEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder3.Create();


        }
    }
}