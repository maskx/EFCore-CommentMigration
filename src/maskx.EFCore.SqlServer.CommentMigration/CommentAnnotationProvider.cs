using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using System.Collections.Generic;
using System.Linq;

namespace maskx.EFCore.SqlServer.CommentMigration
{
    public class CommentAnnotationProvider : SqlServerMigrationsAnnotationProvider
    {
        public CommentAnnotationProvider(MigrationsAnnotationProviderDependencies dependencies)
            : base(dependencies)
        {
        }
        public override IEnumerable<IAnnotation> For(IEntityType entityType)
        {
            var baseAnnotations = base.For(entityType);

            var annotation = entityType.FindAnnotation("Comment");

            return annotation == null
                ? baseAnnotations
                : baseAnnotations.Concat(new[] { annotation });
        }
        public override IEnumerable<IAnnotation> For(IProperty property)
        {
            var baseAnnotations = base.For(property);

            var annotation = property.FindAnnotation("Comment");

            return annotation == null
                ? baseAnnotations
                : baseAnnotations.Concat(new[] { annotation });
        }
    }
}
