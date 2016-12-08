namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moderators : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.ApplicationUserCategories", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCategories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Photos", new[] { "ThreadId" });
            DropIndex("dbo.ApplicationUserCategories", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCategories", new[] { "Category_Id" });
            CreateTable(
                "dbo.Moderators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SubjectId);
            
            AddColumn("dbo.Photos", "PostId", c => c.Int(nullable: false));
            CreateIndex("dbo.Photos", "PostId");
            AddForeignKey("dbo.Photos", "PostId", "dbo.Posts", "Id", cascadeDelete: true);
            DropColumn("dbo.Photos", "ThreadId");
            DropTable("dbo.ApplicationUserCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserCategories",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Category_Id });
            
            AddColumn("dbo.Photos", "ThreadId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Moderators", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Photos", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Moderators", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.Photos", new[] { "PostId" });
            DropIndex("dbo.Moderators", new[] { "SubjectId" });
            DropIndex("dbo.Moderators", new[] { "UserId" });
            DropColumn("dbo.Photos", "PostId");
            DropTable("dbo.Moderators");
            CreateIndex("dbo.ApplicationUserCategories", "Category_Id");
            CreateIndex("dbo.ApplicationUserCategories", "ApplicationUser_Id");
            CreateIndex("dbo.Photos", "ThreadId");
            AddForeignKey("dbo.ApplicationUserCategories", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserCategories", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Photos", "ThreadId", "dbo.Threads", "Id", cascadeDelete: true);
        }
    }
}