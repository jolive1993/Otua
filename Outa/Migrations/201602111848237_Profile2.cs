namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Profile2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Profile", "UserName", c => c.String(maxLength: 256));
            AlterColumn("dbo.Profile", "UserID", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Profile", "UserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Profile", "UserName", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
