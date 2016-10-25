namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fans", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fans", "UserID");
        }
    }
}
