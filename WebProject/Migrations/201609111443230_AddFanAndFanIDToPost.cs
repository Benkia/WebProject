namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFanAndFanIDToPost : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "FanID", "dbo.Fans");
            DropIndex("dbo.Comments", new[] { "FanID" });
            AddColumn("dbo.Posts", "FanID", c => c.Int());
            AlterColumn("dbo.Comments", "FanID", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "FanID");
            CreateIndex("dbo.Posts", "FanID");
            AddForeignKey("dbo.Posts", "FanID", "dbo.Fans", "ID");
            AddForeignKey("dbo.Comments", "FanID", "dbo.Fans", "ID", cascadeDelete: true);
            DropColumn("dbo.Posts", "PostedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "PostedBy", c => c.String());
            DropForeignKey("dbo.Comments", "FanID", "dbo.Fans");
            DropForeignKey("dbo.Posts", "FanID", "dbo.Fans");
            DropIndex("dbo.Posts", new[] { "FanID" });
            DropIndex("dbo.Comments", new[] { "FanID" });
            AlterColumn("dbo.Comments", "FanID", c => c.Int());
            DropColumn("dbo.Posts", "FanID");
            CreateIndex("dbo.Comments", "FanID");
            AddForeignKey("dbo.Comments", "FanID", "dbo.Fans", "ID");
        }
    }
}
