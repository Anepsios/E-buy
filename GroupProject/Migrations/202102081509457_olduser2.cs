namespace GroupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class olduser2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OldUsers", "FirstName", c => c.String());
            AddColumn("dbo.OldUsers", "LastName", c => c.String());
            AddColumn("dbo.OldUsers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OldUsers", "Email");
            DropColumn("dbo.OldUsers", "LastName");
            DropColumn("dbo.OldUsers", "FirstName");
        }
    }
}
