namespace EShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _try : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchases", "name", c => c.String(maxLength: 200));
            AlterColumn("dbo.Purchases", "project", c => c.String(maxLength: 200));
            AlterColumn("dbo.Purchases", "method", c => c.String(maxLength: 200));
            AlterColumn("dbo.Purchases", "category", c => c.String(maxLength: 200));
            AlterColumn("dbo.Purchases", "budget", c => c.Long());
        }

        public override void Down()
        {
            AlterColumn("dbo.Purchases", "budget", c => c.Int());
            AlterColumn("dbo.Purchases", "category", c => c.String(maxLength: 50));
            AlterColumn("dbo.Purchases", "method", c => c.String(maxLength: 50));
            AlterColumn("dbo.Purchases", "project", c => c.String(maxLength: 50));
            AlterColumn("dbo.Purchases", "name", c => c.String(maxLength: 50));
        }
    }
}