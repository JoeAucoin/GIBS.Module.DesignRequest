using GIBS.Module.DesignRequest.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class DetailEntityBuilder : AuditableBaseEntityBuilder<DetailEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_Detail";
        private readonly PrimaryKey<DetailEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_Detail", x => x.DetailId);
        private readonly ForeignKey<DetailEntityBuilder> _moduleForeignKey = new("FK_GIBSDesignRequest_Detail_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public DetailEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }
        protected override DetailEntityBuilder BuildTable(ColumnsBuilder table)
        {
            DetailId = AddAutoIncrementColumn(table, "DetailId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            DetailName = AddStringColumn(table, "DetailName", 250, false);
            DetailDescription = table.Column<string>(name: "DetailDescription", maxLength: int.MaxValue, nullable: true);
            DetailIcon = table.Column<string>(name: "DetailIcon", maxLength: int.MaxValue, nullable: true);
            DetailCode = AddStringColumn(table, "DetailCode", 2);
            DetailColor = AddStringColumn(table, "DetailColor", 50);
            SortOrder = table.Column<int>(name: "SortOrder", nullable: false, defaultValue: 0);
            IsActive = table.Column<bool>(name: "IsActive", nullable: false, defaultValue: true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> DetailId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> DetailName { get; set; }
        public OperationBuilder<AddColumnOperation> DetailDescription { get; set; }
        public OperationBuilder<AddColumnOperation> DetailIcon { get; set; }
        public OperationBuilder<AddColumnOperation> DetailCode { get; set; }
        public OperationBuilder<AddColumnOperation> DetailColor { get; set; }
        public OperationBuilder<AddColumnOperation> SortOrder { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
    }
}

