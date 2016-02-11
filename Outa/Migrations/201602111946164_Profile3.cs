namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Profile3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profile", "Img", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profile", "Img");
        }
    }
}
