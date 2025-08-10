using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class DetailToRequestEntityBuilder : AuditableBaseEntityBuilder<DetailToRequestEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_DetailToRequest";
        private readonly PrimaryKey<DetailToRequestEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_DetailToRequest", x => x.DetailToRequestId);
        private readonly ForeignKey<DetailToRequestEntityBuilder> _designRequestForeignKey = new("FK_GIBSDesignRequest_DetailToRequest", x => x.DesignRequestId, "GIBSDesignRequest", "DesignRequestId", ReferentialAction.Cascade);
        public DetailToRequestEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_designRequestForeignKey);
        }
        protected override DetailToRequestEntityBuilder BuildTable(ColumnsBuilder table)
        {
            DetailToRequestId = AddAutoIncrementColumn(table, "DetailToRequestId");
            DesignRequestId = AddIntegerColumn(table, "DesignRequestId");
            DetailId = AddIntegerColumn(table, "DetailId");
            DetailModel = AddStringColumn(table, "DetailModel", 250, true, true);
            DetailSize = AddStringColumn(table, "DetailSize", 50, true, true);
            DetailColor = AddStringColumn(table, "DetailColor", 50, true, true);

            AddAuditableColumns(table);
            return this;
        }
        public OperationBuilder<AddColumnOperation> DetailToRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> DesignRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> DetailId { get; set; }
        public OperationBuilder<AddColumnOperation> DetailModel { get; set; }
        public OperationBuilder<AddColumnOperation> DetailSize { get; set; }
        public OperationBuilder<AddColumnOperation> DetailColor { get; set; }


    }
}
