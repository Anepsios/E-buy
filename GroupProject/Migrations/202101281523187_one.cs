namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Orders", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "ApplicationUser_Id");
            AddForeignKey("dbo.Orders", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
