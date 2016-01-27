namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "Img", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "Img");
        }
    }
}
