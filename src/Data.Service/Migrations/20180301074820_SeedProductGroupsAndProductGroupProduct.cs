using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using System.Reflection;

namespace Data.Service.Migrations
{
    public partial class SeedProductGroupsAndProductGroupProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName)
                , @"Service\SeedData\exportingProductGroupsAndProductGroupProduct.sql");
            migrationBuilder.Sql(File.ReadAllText(path));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("USE LV_Mini");
            migrationBuilder.Sql("TRUNCATE TABLE [dbo].[ProductGroupProduct]");
            migrationBuilder.Sql("ALTER TABLE [dbo].[ProductGroupProduct] DROP CONSTRAINT [FK_ProductGroupProduct_ProductGroup_IDProductGroup]");
            migrationBuilder.Sql("TRUNCATE TABLE [admin].[ProductGroup]");
            migrationBuilder.Sql("ALTER TABLE [dbo].[ProductGroupProduct] " +
                                 "ADD CONSTRAINT [FK_ProductGroupProduct_ProductGroup_IDProductGroup] " +
                                 "FOREIGN KEY (IDProductGroup) REFERENCES [admin].[ProductGroup]([IDProductGroup])");
        }
    }
}
