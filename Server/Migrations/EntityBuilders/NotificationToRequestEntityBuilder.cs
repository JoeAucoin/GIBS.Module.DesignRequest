using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class NotificationToRequestEntityBuilder : AuditableBaseEntityBuilder<NotificationToRequestEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_NotificationToRequest";
        private readonly PrimaryKey<NotificationToRequestEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_NotificationToRequest", x => x.NotificationToRequestId);
        private readonly ForeignKey<NotificationToRequestEntityBuilder> _designRequestForeignKey = new("FK_GIBSDesignRequest_NotificationToRequest", x => x.DesignRequestId, "GIBSDesignRequest", "DesignRequestId", ReferentialAction.Cascade);
      
        public NotificationToRequestEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_designRequestForeignKey);
        }
        protected override NotificationToRequestEntityBuilder BuildTable(ColumnsBuilder table)
        {
            NotificationToRequestId = AddAutoIncrementColumn(table, "NotificationToRequestId");
            DesignRequestId = AddIntegerColumn(table, "DesignRequestId");
            NotificationId = AddIntegerColumn(table, "NotificationId", true);
            FromUserId = AddIntegerColumn(table, "FromUserId", true);
            FromDisplayName = AddStringColumn(table, "FromDisplayName", 255, true, true);
            FromEmail = AddStringColumn(table, "FromEmail", 256, true, true);
            ToUserId = AddIntegerColumn(table, "ToUserId",true);
            ToDisplayName = AddStringColumn(table, "ToDisplayName", 255, true, true);
            ToEmail = AddStringColumn(table, "ToEmail", 256, true, true);
            Subject = AddStringColumn(table, "Subject", 256, true, true);
            Body = AddMaxStringColumn(table, "Body", true, true);

            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> NotificationToRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> DesignRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> NotificationId { get; set; }
        public OperationBuilder<AddColumnOperation> FromUserId { get; set; }
        public OperationBuilder<AddColumnOperation> FromDisplayName { get; set; }
        public OperationBuilder<AddColumnOperation> FromEmail { get; set; }
        public OperationBuilder<AddColumnOperation> ToUserId { get; set; }
        public OperationBuilder<AddColumnOperation> ToDisplayName { get; set; }
        public OperationBuilder<AddColumnOperation> ToEmail { get; set; }
        public OperationBuilder<AddColumnOperation> Subject { get; set; }
        public OperationBuilder<AddColumnOperation> Body { get; set; }

    }
}

