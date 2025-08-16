using GIBS.Module.DesignRequest.Migrations.EntityBuilders;
using GIBS.Module.DesignRequest.Repository;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;

namespace GIBS.Module.DesignRequest.Migrations
{
    [DbContext(typeof(DesignRequestContext))]
    [Migration("DesignRequest.01.00.03.00")]
    public class AddNotificationToRequest : MultiDatabaseMigration
    {
        public AddNotificationToRequest(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var entityBuilder = new NotificationToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var entityBuilder = new NotificationToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Drop();

        }
    }
}