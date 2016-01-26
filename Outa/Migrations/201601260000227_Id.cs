namespace Outa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Id : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Author = c.String(maxLength: 128),
                        Tags = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Request");
        }
    }
}
