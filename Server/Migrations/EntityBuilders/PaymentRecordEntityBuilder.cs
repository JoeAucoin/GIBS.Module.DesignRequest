using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class PaymentRecordEntityBuilder : AuditableBaseEntityBuilder<PaymentRecordEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_PaymentRecord";
        private readonly PrimaryKey<PaymentRecordEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_PaymentRecord", x => x.PaymentId);
        private readonly ForeignKey<PaymentRecordEntityBuilder> _moduleForeignKey = new("FK_GIBSDesignRequest_PaymentRecord_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);
        private readonly ForeignKey<PaymentRecordEntityBuilder> _userForeignKey = new("FK_GIBSDesignRequest_PaymentRecord_User", x => x.UserId, "User", "UserId", ReferentialAction.NoAction);

        public PaymentRecordEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
            ForeignKeys.Add(_userForeignKey);
        }

        protected override PaymentRecordEntityBuilder BuildTable(ColumnsBuilder table)
        {
            PaymentId = AddAutoIncrementColumn(table, "PaymentId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            UserId = AddIntegerColumn(table, "UserId");
            Provider = AddStringColumn(table, "Provider", 50);
            TransactionId = AddStringColumn(table, "TransactionId", 100);
            Amount = AddDecimalColumn(table, "Amount", 18, 2);
            Status = AddStringColumn(table, "Status", 50);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> PaymentId { get; set; }
        //ModuleId
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> UserId { get; set; }
        public OperationBuilder<AddColumnOperation> Provider { get; set; }
        public OperationBuilder<AddColumnOperation> TransactionId { get; set; }
        public OperationBuilder<AddColumnOperation> Amount { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; }
    }
}