namespace EShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resend : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchases", "counter", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchases", "counter", c => c.Int(nullable: false));
        }
    }
}
