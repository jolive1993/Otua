namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Profile1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        UserID = c.String(nullable: false, maxLength: 128),
                        Lat = c.String(),
                        Long = c.String(),
                        Description = c.String(),
                        Rating = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Profile");
        }
    }
}
