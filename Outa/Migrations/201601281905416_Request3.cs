namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "Lat", c => c.String());
            AddColumn("dbo.Request", "Long", c => c.String());
        }
        
        public override void Down()
        {

        }
    }
}
