namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class swag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Review",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        UserName = c.String(maxLength: 256),
                        Content = c.String(),
                        Rating = c.Decimal(precision: 18, scale: 2),
                        TrnsactionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(),
                        OfferId = c.Int(),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transaction");
            DropTable("dbo.Review");
        }
    }
}
