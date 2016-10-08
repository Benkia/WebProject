namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFanIDToFan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Fan_ID", c => c.Int());
            CreateIndex("dbo.Comments", "Fan_ID");
            AddForeignKey("dbo.Comments", "Fan_ID", "dbo.Fans", "ID");
            DropColumn("dbo.Comments", "CommentedBy");
            DropColumn("dbo.Comments", "FanID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "FanID", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "CommentedBy", c => c.String());
            DropForeignKey("dbo.Comments", "Fan_ID", "dbo.Fans");
            DropIndex("dbo.Comments", new[] { "Fan_ID" });
            DropColumn("dbo.Comments", "Fan_ID");
        }
    }
}
