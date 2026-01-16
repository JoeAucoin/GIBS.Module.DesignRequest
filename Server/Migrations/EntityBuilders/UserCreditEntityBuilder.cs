using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class UserCreditEntityBuilder : AuditableBaseEntityBuilder<UserCreditEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_UserCredit";
        private readonly PrimaryKey<UserCreditEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_UserCredit", x => x.UserCreditId);
        private readonly ForeignKey<UserCreditEntityBuilder> _userForeignKey = new("FK_GIBSDesignRequest_UserCredit_User", x => x.UserId, "User", "UserId", ReferentialAction.NoAction);
        private readonly ForeignKey<UserCreditEntityBuilder> _moduleForeignKey = new("FK_GIBSDesignRequest_UserCredit_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public UserCreditEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_userForeignKey);
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override UserCreditEntityBuilder BuildTable(ColumnsBuilder table)
        {
            UserCreditId = AddAutoIncrementColumn(table, "UserCreditId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            UserId = AddIntegerColumn(table, "UserId");
            CreditBalance = AddIntegerColumn(table, "CreditBalance");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> UserCreditId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> UserId { get; set; }
        public OperationBuilder<AddColumnOperation> CreditBalance { get; set; }
    }
}