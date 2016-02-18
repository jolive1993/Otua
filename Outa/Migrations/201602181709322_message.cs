namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class message : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Author = c.String(maxLength: 128),
                        AuthorUn = c.String(maxLength: 256),
                        Recp = c.String(maxLength: 128),
                        RecpUn = c.String(maxLength: 256),
                        Date = c.DateTime(nullable: false),
                        RequestID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Message");
        }
    }
}
