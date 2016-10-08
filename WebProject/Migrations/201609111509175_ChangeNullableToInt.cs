namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNullableToInt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "FanID", "dbo.Fans");
            DropIndex("dbo.Posts", new[] { "FanID" });
            AlterColumn("dbo.Posts", "FanID", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "FanID");
            AddForeignKey("dbo.Posts", "FanID", "dbo.Fans", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "FanID", "dbo.Fans");
            DropIndex("dbo.Posts", new[] { "FanID" });
            AlterColumn("dbo.Posts", "FanID", c => c.Int());
            CreateIndex("dbo.Posts", "FanID");
            AddForeignKey("dbo.Posts", "FanID", "dbo.Fans", "ID");
        }
    }
}
