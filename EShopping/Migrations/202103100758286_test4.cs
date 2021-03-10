namespace EShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchases", "name", c => c.String());
            AlterColumn("dbo.Purchases", "project", c => c.String());
            AlterColumn("dbo.Purchases", "method", c => c.String());
            AlterColumn("dbo.Purchases", "category", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchases", "category", c => c.String(maxLength: 200));
            AlterColumn("dbo.Purchases", "method", c => c.String(maxLength: 200));
            AlterColumn("dbo.Purchases", "project", c => c.String(maxLength: 200));
            AlterColumn("dbo.Purchases", "name", c => c.String(maxLength: 200));
        }
    }
}
