namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "Title", c => c.String(maxLength: 36));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "Title");
        }
    }
}
