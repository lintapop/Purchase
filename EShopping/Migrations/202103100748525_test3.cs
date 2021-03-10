namespace EShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchases", "counter", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchases", "counter", c => c.String(maxLength: 100));
        }
    }
}
