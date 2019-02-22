using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace maskx.EFCore.SqlServer.CommentMigration
{
    public class CommentDbContext : DbContext
    {
        public CommentDbContext() : base()
        {

        }
        public CommentDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ReplaceService<IMigrationsSqlGenerator, SqlServerCommentMigrationsSqlGenerator>()
                .ReplaceService<IMigrationsAnnotationProvider, CommentAnnotationProvider>();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddCommentAnnotation();
            base.OnModelCreating(modelBuilder);
        }
    }
}
