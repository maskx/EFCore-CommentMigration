using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Linq;

namespace maskx.EFCore.SqlServer.CommentMigration
{
    public class SqlServerCommentMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        const string CommentAnnotationName = "Comment";
        public SqlServerCommentMigrationsSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
            IMigrationsAnnotationProvider migrationsAnnotations)
            : base(dependencies, migrationsAnnotations)
        {
        }
        protected override void Generate(AlterTableOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);
            IAnnotation comment = operation.GetAnnotations().FirstOrDefault(x => x.Name == CommentAnnotationName);
            if (comment is null)
            {
                comment = operation.OldTable.GetAnnotation(CommentAnnotationName);
                if (comment != null)
                    DropTableComment(builder, operation.Schema, operation.Name);
            }
            else
            {
                UpdateTableComment(builder, operation.Schema, operation.Name, comment.Value);
            }
        }
        protected override void Generate(CreateTableOperation operation, IModel model,
          MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);
            var tableAnnotations = operation.GetAnnotations().FirstOrDefault(x => x.Name == CommentAnnotationName);
            if (tableAnnotations != null)
            {
                AddTableComment(builder, operation.Schema, operation.Name, tableAnnotations.Value);
            }
            foreach (var column in operation.Columns)
            {
                var commentAnnotation = column.FindAnnotation(CommentAnnotationName);
                if (commentAnnotation is null)
                    continue;
                AddColumnComment(builder, operation.Schema, operation.Name, column.Name, commentAnnotation.Value);
            }

            builder.EndCommand();
        }
        protected override void Generate(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);
            var comment = operation.GetAnnotations().FirstOrDefault(x => x.Name == CommentAnnotationName);
            if (comment != null)
                AddColumnComment(builder, operation.Schema, operation.Table, operation.Name, comment.Value);
        }
        protected override void Generate(AlterColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);
            IAnnotation comment = operation.GetAnnotations().FirstOrDefault(x => x.Name == CommentAnnotationName);
            if (comment is null)
            {
                comment = operation.OldColumn.GetAnnotation(CommentAnnotationName);
                if (comment != null)
                    DropTableComment(builder, operation.Schema, operation.Name);
            }
            else
            {
                UpdateColumnComment(builder, operation.Schema, operation.Table, operation.Name, comment.Value);
            }
        }
        private void AddTableComment(MigrationCommandListBuilder builder, string schema, string table, object desc)
        {
            builder.Comment("sp_addextendedproperty", schema, table, string.Empty, desc);
        }
        private void UpdateTableComment(MigrationCommandListBuilder builder, string schema, string table, object desc)
        {
            builder.Comment("sp_updateextendedproperty", schema, table, string.Empty, desc);
        }
        private void DropTableComment(MigrationCommandListBuilder builder, string schema, string table)
        {
            builder.Comment("sp_dropextendedproperty", schema, table, string.Empty, string.Empty);
        }
        private void AddColumnComment(MigrationCommandListBuilder builder, string schema, string table, string cloumn, object desc)
        {
            builder.Comment("sp_addextendedproperty", schema, table, cloumn, desc);
        }
        private void UpdateColumnComment(MigrationCommandListBuilder builder, string schema, string table, string cloumn, object desc)
        {
            builder.Comment("sp_updateextendedproperty", schema, table, cloumn, desc);
        }
        private void DropColumnComment(MigrationCommandListBuilder builder, string schema, string table, string cloumn)
        {
            builder.Comment("sp_dropextendedproperty", schema, table, cloumn, string.Empty);
        }
    }
}
