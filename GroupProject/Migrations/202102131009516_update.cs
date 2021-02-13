namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OldUsers",
                c => new
                    {
                        OldUserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OldUserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OldUsers");
        }
    }
}
