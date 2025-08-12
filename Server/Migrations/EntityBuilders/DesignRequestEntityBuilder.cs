using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class DesignRequestEntityBuilder : AuditableBaseEntityBuilder<DesignRequestEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest";
        private readonly PrimaryKey<DesignRequestEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest", x => x.DesignRequestId);
        private readonly ForeignKey<DesignRequestEntityBuilder> _moduleForeignKey = new("FK_GIBSDesignRequest_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public DesignRequestEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override DesignRequestEntityBuilder BuildTable(ColumnsBuilder table)
        {
            DesignRequestId = AddAutoIncrementColumn(table,"DesignRequestId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            ContactName = AddMaxStringColumn(table, "ContactName");
            Company = AddStringColumn(table, "Company", 100, true, true);
            ProjectName = AddStringColumn(table, "ProjectName", 100, true, true);

            Address = AddMaxStringColumn(table, "Address", true, true);
            Phone = AddStringColumn(table, "Phone", 50, true, true);
            Email = AddStringColumn(table, "Email", 50, true, true);
            Website = AddStringColumn(table, "Website", 100, true, true);
            QuestionComments = AddMaxStringColumn(table, "QuestionComments", true, true);
            Interest = AddMaxStringColumn(table, "Interest", true, true);
            InstallationDate = AddDateTimeColumn(table, "InstallationDate", true);
            OverallSpaceDimensions = AddStringColumn(table, "OverallSpaceDimensions", 50, true, true);
            CeilingHeight = AddStringColumn(table, "CeilingHeight", 50, true, true);
            LengthOfKitchen = AddStringColumn(table, "LengthOfKitchen", 50, true, true);
            SlopeOfPatio = AddStringColumn(table, "SlopeOfPatio", 50, true, true);
            ShapeConfiguration = AddStringColumn(table, "ShapeConfiguration", 255, true, true);
            DoorStyle = AddStringColumn(table, "DoorStyle", 100, true, true);
            Color = AddStringColumn(table, "Color", 50, true, true);
            CountertopThickness = AddStringColumn(table, "CountertopThickness", 50, true, true);
            CounterDepth = AddStringColumn(table, "CounterDepth", 50, true, true);
            CounterHeight = AddStringColumn(table, "CounterHeight", 50, true, true);
            Status = AddStringColumn(table, "Status", 50, true, true);
            IP_Address = AddStringColumn(table, "IP_Address", 45, true, true);
            AssignedToUserId = AddIntegerColumn(table, "AssignedToUserId", true, -1);
            

            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> DesignRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> ContactName { get; set; }
        public OperationBuilder<AddColumnOperation> Company { get; set; }
        public OperationBuilder<AddColumnOperation> ProjectName { get; set; }
        public OperationBuilder<AddColumnOperation> Address { get; set; }
        public OperationBuilder<AddColumnOperation> Phone { get; set; }
        public OperationBuilder<AddColumnOperation> Email { get; set; }
        public OperationBuilder<AddColumnOperation> Website { get; set; }
        public OperationBuilder<AddColumnOperation> QuestionComments { get; set; }
        public OperationBuilder<AddColumnOperation> Interest { get; set; }
        public OperationBuilder<AddColumnOperation> InstallationDate { get; set; }
        public OperationBuilder<AddColumnOperation> OverallSpaceDimensions { get; set; }
        public OperationBuilder<AddColumnOperation> CeilingHeight { get; set; }
        public OperationBuilder<AddColumnOperation> LengthOfKitchen { get; set; }
        public OperationBuilder<AddColumnOperation> SlopeOfPatio { get; set; }
        public OperationBuilder<AddColumnOperation> ShapeConfiguration { get; set; }
        public OperationBuilder<AddColumnOperation> DoorStyle { get; set; }
        public OperationBuilder<AddColumnOperation> Color { get; set; }
        public OperationBuilder<AddColumnOperation> CountertopThickness { get; set; }
        public OperationBuilder<AddColumnOperation> CounterDepth { get; set; }
        public OperationBuilder<AddColumnOperation> CounterHeight { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; } 
        public OperationBuilder<AddColumnOperation> IP_Address { get; set; }
        public OperationBuilder<AddColumnOperation> AssignedToUserId { get; set; }


    }
}
