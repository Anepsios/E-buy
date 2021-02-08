namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oldusers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OldUsers",
                c => new
                    {
                        OldUserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.OldUserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OldUsers");
        }
    }
}
