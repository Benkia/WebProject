namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Blank1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "FanID", "dbo.Fans");
            AddForeignKey("dbo.Comments", "FanID", "dbo.Fans", "ID", cascadeDelete: false);

            DropForeignKey("dbo.Posts", "FanID", "dbo.Fans");
            AddForeignKey("dbo.Posts", "FanID", "dbo.Fans", "ID", cascadeDelete: true);

            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            AddForeignKey("dbo.Comments", "PostID", "dbo.Posts", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
        }
    }
}
