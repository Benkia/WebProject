namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFanIDToComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "FanID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "FanID");
        }
    }
}
