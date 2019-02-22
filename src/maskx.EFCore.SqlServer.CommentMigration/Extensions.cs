using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;
using System.Xml.Linq;

namespace maskx.EFCore.SqlServer.CommentMigration
{
    public static class Extensions
    {
        public static void AddCommentAnnotation(this ModelBuilder modelBuilder)
        {
            var assembly = modelBuilder.Model.GetEntityTypes().First().ClrType.Assembly;
            string fileLocation = assembly.Location.Remove(assembly.Location.LastIndexOf('.')) + ".xml";
            XElement ele = XElement.Load(fileLocation);
            var members = ele.Element("members");
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                string tName = string.Format("T:{0}", entity.Name);
                var eTComment = members.Elements().Where(e => e.Attribute("name").Value == tName);
                if (eTComment.Count() > 0)
                    entity.AddAnnotation("Comment", eTComment.First().Value.Replace("\r\n", " "));
                foreach (var prop in entity.GetProperties())
                {
                    string pName = string.Format("P:{0}.{1}", entity.Name, prop.Name);
                    var ePComment = members.Elements().Where(e => e.Attribute("name").Value == pName);
                    if (ePComment.Count() > 0)
                        prop.AddAnnotation("Comment", ePComment.First().Value.Replace("\r\n", " "));
                }
            }
        }

        public static void Comment(this MigrationCommandListBuilder builder, string cmd,string schema, string table, string column, object desc)
        {
            builder.AppendLine("EXEC ");
            builder.Append(cmd);
            builder.AppendLine("@name = N'MS_Description', @value =");
            if (!cmd.StartsWith("sp_drop", System.StringComparison.OrdinalIgnoreCase))
                builder.Append(string.Format("N'{0}'", desc));
            builder.Append(",@level0type = N'Schema', @level0name =");
            builder.Append(string.Format("N'{0}'", string.IsNullOrEmpty(schema) ? "dbo" : schema));
            builder.Append(",@level1type = N'Table',  @level1name =");
            builder.Append(string.Format("N'{0}'", table));
            if (!string.IsNullOrEmpty(column))
            {
                builder.Append(",@level2type = N'Column', @level2name =");
                builder.Append(string.Format("N'{0}'", column));
            }
            builder.Append(";");
        }
    }
}
