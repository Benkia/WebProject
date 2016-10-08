namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsersFanID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FanID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FanID");
        }
    }
}
