namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IDontKnow : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "FanID", "dbo.Fans");
            DropIndex("dbo.Comments", new[] { "FanID" });
            AlterColumn("dbo.Comments", "FanID", c => c.Int());
            CreateIndex("dbo.Comments", "FanID");
            AddForeignKey("dbo.Comments", "FanID", "dbo.Fans", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "FanID", "dbo.Fans");
            DropIndex("dbo.Comments", new[] { "FanID" });
            AlterColumn("dbo.Comments", "FanID", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "FanID");
            AddForeignKey("dbo.Comments", "FanID", "dbo.Fans", "ID", cascadeDelete: true);
        }
    }
}
