using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using GIBS.Module.DesignRequest.Migrations.EntityBuilders;
using GIBS.Module.DesignRequest.Repository;

namespace GIBS.Module.DesignRequest.Migrations
{
    [DbContext(typeof(DesignRequestContext))]
    [Migration("GIBS.Module.DesignRequest.01.00.00.00")]
    public class InitializeModule : MultiDatabaseMigration
    {
        public InitializeModule(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var entityBuilder = new DesignRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Create();

            var detailEntityBuilder = new DetailEntityBuilder(migrationBuilder, ActiveDatabase);
            detailEntityBuilder.Create();

            var detailToRequestEntityBuilder = new DetailToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            detailToRequestEntityBuilder.Create();

            var applianceEntityBuilder = new ApplianceEntityBuilder(migrationBuilder, ActiveDatabase);
            applianceEntityBuilder.Create();

            var applianceToRequestEntityBuilder = new ApplianceToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            applianceToRequestEntityBuilder.Create();

            var fileToRequestEntityBuilder = new FileToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            fileToRequestEntityBuilder.Create();

            var noteToRequestEntityBuilder = new NoteToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            noteToRequestEntityBuilder.Create();

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var noteToRequestEntityBuilder = new NoteToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            noteToRequestEntityBuilder.Drop();

            var fileToRequestEntityBuilder = new FileToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            fileToRequestEntityBuilder.Drop();

            var applianceToRequestEntityBuilder = new ApplianceToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            applianceToRequestEntityBuilder.Drop();

            var applianceEntityBuilder = new ApplianceEntityBuilder(migrationBuilder, ActiveDatabase);
            applianceEntityBuilder.Drop();

            var detailToRequestEntityBuilder = new DetailToRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            detailToRequestEntityBuilder.Drop();

            var detailEntityBuilder = new DetailEntityBuilder(migrationBuilder, ActiveDatabase);
            detailEntityBuilder.Drop();

            var entityBuilder = new DesignRequestEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Drop();
        }
    }
}
