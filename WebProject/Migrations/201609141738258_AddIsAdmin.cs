namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "isAdmin", c => c.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "isAdmin");
        }
    }
}
