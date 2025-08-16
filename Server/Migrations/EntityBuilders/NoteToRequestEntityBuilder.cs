using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class NoteToRequestEntityBuilder : AuditableBaseEntityBuilder<NoteToRequestEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_NoteToRequest";
        private readonly PrimaryKey<NoteToRequestEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_NoteToRequest", x => x.NoteId);
        private readonly ForeignKey<NoteToRequestEntityBuilder> _designRequestForeignKey = new("FK_GIBSDesignRequest_NoteToRequest", x => x.DesignRequestId, "GIBSDesignRequest", "DesignRequestId", ReferentialAction.Cascade);
        public NoteToRequestEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_designRequestForeignKey);
        }
        protected override NoteToRequestEntityBuilder BuildTable(ColumnsBuilder table)
        {
            NoteId = AddAutoIncrementColumn(table, "NoteId");
            DesignRequestId = AddIntegerColumn(table, "DesignRequestId");
            Note = AddMaxStringColumn(table, "Note");
            
            AddAuditableColumns(table);
            return this;
        }
        public OperationBuilder<AddColumnOperation> NoteId { get; set; }
        public OperationBuilder<AddColumnOperation> DesignRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> Note { get; set; }


    }
}
