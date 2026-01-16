using GIBS.Module.DesignRequest.Migrations.EntityBuilders;
using GIBS.Module.DesignRequest.Repository;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;

namespace GIBS.Module.DesignRequest.Server.Migrations
{
    [DbContext(typeof(DesignRequestContext))]
    [Migration("GIBS.Module.DesignRequest.01.00.06.00")]

    public class ModifyColumnsCreditTransaction_06 : MultiDatabaseMigration
    {
        public ModifyColumnsCreditTransaction_06(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "GIBSDesignRequest_CreditTransaction",
                newName: "Credits");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "GIBSDesignRequest_CreditTransaction",
                type: "decimal(18, 2)", 
                nullable: false,
                defaultValue: 0m);

           
            migrationBuilder.AddColumn<string>(
                name: "PP_PaymentId",
                table: "GIBSDesignRequest_PaymentRecord",
                maxLength: 50,
                nullable: true);
           
            migrationBuilder.AddColumn<string>(
                name: "PP_Response",
                table: "GIBSDesignRequest_PaymentRecord",
                type: "nvarchar(MAX)",
                nullable: true, 
                defaultValue: "");

            
            migrationBuilder.AddColumn<string>(
                name: "PaypalPaymentState",
                table: "GIBSDesignRequest_PaymentRecord",
                maxLength: 50,
                nullable: true);



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credits",
                table: "GIBSDesignRequest_CreditTransaction");

            migrationBuilder.DropColumn(
                name: "Price",
               table: "GIBSDesignRequest_CreditTransaction");

           


        }
    }
}