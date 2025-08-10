//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GIBS.Module.DesignRequest.Server.Migrations.EntityBuilders
//{
//    internal class ApplianceToRequestEntityBuilder
//    {
//    }
//}
using GIBS.Module.DesignRequest.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class ApplianceToRequestEntityBuilder : AuditableBaseEntityBuilder<ApplianceToRequestEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_ApplianceToRequest";
        private readonly PrimaryKey<ApplianceToRequestEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_ApplianceToRequest", x => x.ApplianceToRequestId);
        private readonly ForeignKey<ApplianceToRequestEntityBuilder> _designRequestForeignKey = new("FK_GIBSDesignRequest_ApplianceToRequest", x => x.DesignRequestId, "GIBSDesignRequest", "DesignRequestId", ReferentialAction.Cascade);
        public ApplianceToRequestEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_designRequestForeignKey);
        }
        protected override ApplianceToRequestEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ApplianceToRequestId = AddAutoIncrementColumn(table, "ApplianceToRequestId");
            DesignRequestId = AddIntegerColumn(table, "DesignRequestId");
            ApplianceId = AddIntegerColumn(table, "ApplianceId");
            BrandModel = AddStringColumn(table, "BrandModel", 250, true, true);
            Size = AddStringColumn(table, "Size", 50, true, true);
            FuelType = AddStringColumn(table, "FuelType", 50, true, true);

            AddAuditableColumns(table);
            return this;
        }
        public OperationBuilder<AddColumnOperation> ApplianceToRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> DesignRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> ApplianceId { get; set; }
        public OperationBuilder<AddColumnOperation> BrandModel { get; set; }
        public OperationBuilder<AddColumnOperation> Size { get; set; }
        public OperationBuilder<AddColumnOperation> FuelType { get; set; }


    }
}
