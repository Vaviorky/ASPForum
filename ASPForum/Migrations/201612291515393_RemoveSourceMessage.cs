namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSourceMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Photos", "Post_Id", "dbo.Posts");
            DropIndex("dbo.Photos", new[] { "ThreadId" });
            DropIndex("dbo.Photos", new[] { "Post_Id" });
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.String(nullable: false, maxLength: 70),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
            DropColumn("dbo.Messages", "Source");
            DropTable("dbo.Photos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 70),
                        Text = c.String(),
                        Source = c.String(),
                        ThreadId = c.Int(nullable: false),
                        Post_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Messages", "Source", c => c.String());
            DropForeignKey("dbo.Attachments", "PostId", "dbo.Posts");
            DropIndex("dbo.Attachments", new[] { "PostId" });
            DropTable("dbo.Attachments");
            CreateIndex("dbo.Photos", "Post_Id");
            CreateIndex("dbo.Photos", "ThreadId");
            AddForeignKey("dbo.Photos", "Post_Id", "dbo.Posts", "Id");
            AddForeignKey("dbo.Photos", "ThreadId", "dbo.Threads", "Id", cascadeDelete: true);
        }
    }
}
