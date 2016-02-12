namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class swagit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "Status");
        }
    }
}
