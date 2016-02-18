namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class readstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offer", "ReadStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offer", "ReadStatus");
        }
    }
}
