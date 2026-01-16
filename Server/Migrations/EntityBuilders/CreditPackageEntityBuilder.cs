using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class CreditPackageEntityBuilder : AuditableBaseEntityBuilder<CreditPackageEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_CreditPackage";
        private readonly PrimaryKey<CreditPackageEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_CreditPackage", x => x.CreditPackageId);
        private readonly ForeignKey<CreditPackageEntityBuilder> _moduleForeignKey = new("FK_GIBSDesignRequest_CreditPackage_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public CreditPackageEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override CreditPackageEntityBuilder BuildTable(ColumnsBuilder table)
        {
            CreditPackageId = AddAutoIncrementColumn(table, "CreditPackageId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            Name = AddStringColumn(table, "Name", 255);
            Credits = AddIntegerColumn(table, "Credits");
            Price = table.Column<decimal>(name: "Price", type: "decimal(18,2)", nullable: false);
            Description = AddStringColumn(table, "Description", 500); // Assuming length, adjust if needed
            IsActive = AddBooleanColumn(table, "IsActive");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> CreditPackageId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }

        public OperationBuilder<AddColumnOperation> Name { get; set; }
        public OperationBuilder<AddColumnOperation> Credits { get; set; }
        public OperationBuilder<AddColumnOperation> Price { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
    }
}