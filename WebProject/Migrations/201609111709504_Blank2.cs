namespace WebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Blank2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            AddForeignKey("dbo.Comments", "PostID", "dbo.Posts", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
        }
    }
}
