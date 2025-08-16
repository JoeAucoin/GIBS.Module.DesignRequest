using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.DesignRequest.Migrations.EntityBuilders
{
    public class FileToRequestEntityBuilder : AuditableBaseEntityBuilder<FileToRequestEntityBuilder>
    {
        private const string _entityTableName = "GIBSDesignRequest_FileToRequest";
        private readonly PrimaryKey<FileToRequestEntityBuilder> _primaryKey = new("PK_GIBSDesignRequest_FileToRequest", x => x.FileToRequestId);
        private readonly ForeignKey<FileToRequestEntityBuilder> _designRequestForeignKey = new("FK_GIBSDesignRequest_FileToRequest", x => x.DesignRequestId, "GIBSDesignRequest", "DesignRequestId", ReferentialAction.Cascade);
        private readonly ForeignKey<FileToRequestEntityBuilder> _fileIdForeignKey = new("FK_File_FileToRequest", x => x.FileId, "File", "FileId", ReferentialAction.NoAction);
        public FileToRequestEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_designRequestForeignKey);
            ForeignKeys.Add(_fileIdForeignKey);
        }
        protected override FileToRequestEntityBuilder BuildTable(ColumnsBuilder table)
        {
            FileToRequestId = AddAutoIncrementColumn(table, "FileToRequestId");
            DesignRequestId = AddIntegerColumn(table, "DesignRequestId");
            FileId = AddIntegerColumn(table, "FileId");
            Name = AddStringColumn(table, "Name", 100, false, true);
            Description = AddStringColumn(table, "Description", 255, true, true);
            FilePath = AddStringColumn(table, "FilePath", 255, true, true);

            AddAuditableColumns(table);
            return this;
        }
        public OperationBuilder<AddColumnOperation> FileToRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> DesignRequestId { get; set; }
        public OperationBuilder<AddColumnOperation> FileId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> FilePath { get; set; }


    }
}
