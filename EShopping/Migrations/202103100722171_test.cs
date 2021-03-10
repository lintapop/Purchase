namespace EShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchases", "announce_date", c => c.DateTime());
            AlterColumn("dbo.Purchases", "submit_deadline", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchases", "submit_deadline", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Purchases", "announce_date", c => c.DateTime(nullable: false));
        }
    }
}
