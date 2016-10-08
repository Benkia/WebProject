namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFanAndFanID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Fan_ID", "dbo.Fans");
            DropIndex("dbo.Comments", new[] { "Fan_ID" });
            RenameColumn(table: "dbo.Comments", name: "Fan_ID", newName: "FanID");
            AlterColumn("dbo.Comments", "FanID", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "FanID");
            AddForeignKey("dbo.Comments", "FanID", "dbo.Fans", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "FanID", "dbo.Fans");
            DropIndex("dbo.Comments", new[] { "FanID" });
            AlterColumn("dbo.Comments", "FanID", c => c.Int());
            RenameColumn(table: "dbo.Comments", name: "FanID", newName: "Fan_ID");
            CreateIndex("dbo.Comments", "Fan_ID");
            AddForeignKey("dbo.Comments", "Fan_ID", "dbo.Fans", "ID");
        }
    }
}
