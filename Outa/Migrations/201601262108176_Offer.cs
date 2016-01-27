namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Offer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offer", "o_Status", c => c.Int(nullable: false));
            AddColumn("dbo.Request", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "Status");
            DropColumn("dbo.Offer", "o_Status");
        }
    }
}
