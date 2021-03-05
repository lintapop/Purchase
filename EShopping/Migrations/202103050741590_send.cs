namespace EShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class send : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 50),
                        project = c.String(maxLength: 50),
                        counter = c.Int(nullable: false),
                        method = c.String(maxLength: 50),
                        category = c.String(maxLength: 50),
                        announce_date = c.DateTime(nullable: false),
                        submit_deadline = c.DateTime(nullable: false),
                        budget = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Purchases");
        }
    }
}
