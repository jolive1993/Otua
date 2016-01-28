namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Request", "Title", c => c.String(nullable: false, maxLength: 36));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Request", "Title", c => c.String(maxLength: 36));
        }
    }
}
