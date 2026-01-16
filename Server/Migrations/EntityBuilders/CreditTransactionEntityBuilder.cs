using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;
using System;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class CreditTransactionEntityBuilder : AuditableBaseEntityBuilder<CreditTransactionEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_CreditTransaction";
        private readonly PrimaryKey<CreditTransactionEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_CreditTransaction", x => x.TransactionId);
        private readonly ForeignKey<CreditTransactionEntityBuilder> _userForeignKey = new("FK_GIBSDesignRequest_CreditTransaction_User", x => x.UserId, "User", "UserId", ReferentialAction.NoAction);
        // Optional FK to DesignRequest - using NoAction to strictly avoid cycles or cascade issues on optional relationships
        private readonly ForeignKey<CreditTransactionEntityBuilder> _designRequestForeignKey = new("FK_GIBSCreditTransaction_DesignRequest", x => x.DesignRequestId, "GIBSDesignRequest", "DesignRequestId", ReferentialAction.Cascade);

        public CreditTransactionEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_userForeignKey);
            ForeignKeys.Add(_designRequestForeignKey);
        }

        protected override CreditTransactionEntityBuilder BuildTable(ColumnsBuilder table)
        {
            TransactionId = AddAutoIncrementColumn(table, "TransactionId");
            UserId = AddIntegerColumn(table, "UserId");
            Amount = AddIntegerColumn(table, "Amount");
            TransactionType = AddStringColumn(table, "TransactionType", 50);
            DesignRequestId = AddIntegerColumn(table, "DesignRequestId", true); // nullable = true
            TransactionDate = table.Column<DateTime>(name: "TransactionDate", nullable: false);
            Notes = AddMaxStringColumn(table, "Notes");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> TransactionId { get; set; }
        public OperationBuilder<AddColumnOperation> UserId { get; set; }
        public OperationBuilder<AddColumnOperation> Amount { get; set; }
        public OperationBuilder<AddColumnOperation> TransactionType { get; set; }
        public OperationBuilder<AddColumnOperation> DesignRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> TransactionDate { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
    }
}