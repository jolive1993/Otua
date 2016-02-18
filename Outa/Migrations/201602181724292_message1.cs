namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class message1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "Subject", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Message", "Subject");
        }
    }
}
