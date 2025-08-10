using GIBS.Module.DesignRequest.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class ApplianceEntityBuilder : AuditableBaseEntityBuilder<ApplianceEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_Appliance";
        private readonly PrimaryKey<ApplianceEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_Appliance", x => x.ApplianceId);
        private readonly ForeignKey<ApplianceEntityBuilder> _moduleForeignKey = new("FK_GIBSDesignRequest_Appliance_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public ApplianceEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }
        protected override ApplianceEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ApplianceId = AddAutoIncrementColumn(table, "ApplianceId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            ApplianceName = AddStringColumn(table, "ApplianceName", 250, false);
            ApplianceDescription = table.Column<string>(name: "ApplianceDescription", maxLength: int.MaxValue, nullable: true);
            ApplianceIcon = table.Column<string>(name: "ApplianceIcon", maxLength: int.MaxValue, nullable: true);
            ApplianceCode = AddStringColumn(table, "ApplianceCode", 2);
            ApplianceColor = AddStringColumn(table, "ApplianceColor", 50);
            SortOrder = table.Column<int>(name: "SortOrder", nullable: false, defaultValue: 0);
            IsActive = table.Column<bool>(name: "IsActive", nullable: false, defaultValue: true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> ApplianceId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> ApplianceName { get; set; }
        public OperationBuilder<AddColumnOperation> ApplianceDescription { get; set; }
        public OperationBuilder<AddColumnOperation> ApplianceIcon { get; set; }
        public OperationBuilder<AddColumnOperation> ApplianceCode { get; set; }
        public OperationBuilder<AddColumnOperation> ApplianceColor { get; set; }
        public OperationBuilder<AddColumnOperation> SortOrder { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
    }
}

