namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Threads", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "Thread_Id", "dbo.Threads");
            DropIndex("dbo.Posts", new[] { "Thread_Id" });
            DropIndex("dbo.Threads", new[] { "User_Id" });
            RenameColumn(table: "dbo.Subjects", name: "Category_Id", newName: "CategoryId");
            RenameColumn(table: "dbo.Posts", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Threads", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Posts", name: "Thread_Id", newName: "ThreadId");
            RenameColumn(table: "dbo.Photos", name: "Post_Id", newName: "ThreadId");
            RenameColumn(table: "dbo.Threads", name: "Subject_Id", newName: "SubjectId");
            RenameColumn(table: "dbo.News", name: "User_id_Id", newName: "UserId");
            RenameIndex(table: "dbo.Posts", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Threads", name: "IX_Subject_Id", newName: "IX_SubjectId");
            RenameIndex(table: "dbo.Photos", name: "IX_Post_Id", newName: "IX_ThreadId");
            RenameIndex(table: "dbo.Subjects", name: "IX_Category_Id", newName: "IX_CategoryId");
            RenameIndex(table: "dbo.News", name: "IX_User_id_Id", newName: "IX_UserId");
            AlterColumn("dbo.Posts", "ThreadId", c => c.Int(nullable: false));
            AlterColumn("dbo.Threads", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "ThreadId");
            CreateIndex("dbo.Threads", "UserId");
            AddForeignKey("dbo.Threads", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Posts", "ThreadId", "dbo.Threads", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Threads", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Threads", new[] { "UserId" });
            DropIndex("dbo.Posts", new[] { "ThreadId" });
            AlterColumn("dbo.Threads", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Posts", "ThreadId", c => c.Int());
            RenameIndex(table: "dbo.News", name: "IX_UserId", newName: "IX_User_id_Id");
            RenameIndex(table: "dbo.Subjects", name: "IX_CategoryId", newName: "IX_Category_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_ThreadId", newName: "IX_Post_Id");
            RenameIndex(table: "dbo.Threads", name: "IX_SubjectId", newName: "IX_Subject_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.News", name: "UserId", newName: "User_id_Id");
            RenameColumn(table: "dbo.Threads", name: "SubjectId", newName: "Subject_Id");
            RenameColumn(table: "dbo.Photos", name: "ThreadId", newName: "Post_Id");
            RenameColumn(table: "dbo.Posts", name: "ThreadId", newName: "Thread_Id");
            RenameColumn(table: "dbo.Threads", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Subjects", name: "CategoryId", newName: "Category_Id");
            CreateIndex("dbo.Threads", "User_Id");
            CreateIndex("dbo.Posts", "Thread_Id");
            AddForeignKey("dbo.Posts", "Thread_Id", "dbo.Threads", "Id");
            AddForeignKey("dbo.Threads", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
