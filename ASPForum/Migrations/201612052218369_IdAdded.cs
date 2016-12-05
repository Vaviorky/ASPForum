namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "Thread_Id", "dbo.Threads");
            DropIndex("dbo.Posts", new[] { "Thread_Id" });
            RenameColumn(table: "dbo.Subjects", name: "Category_Id", newName: "CategoryId");
            RenameColumn(table: "dbo.Posts", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Threads", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Posts", name: "Thread_Id", newName: "ThreadId");
            RenameColumn(table: "dbo.Photos", name: "Post_Id", newName: "ThreadId");
            RenameColumn(table: "dbo.Threads", name: "Subject_Id", newName: "SubjectId");
            RenameColumn(table: "dbo.Friends", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.News", name: "User_id_Id", newName: "UserId");
            RenameIndex(table: "dbo.Posts", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Threads", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Threads", name: "IX_Subject_Id", newName: "IX_SubjectId");
            RenameIndex(table: "dbo.Photos", name: "IX_Post_Id", newName: "IX_ThreadId");
            RenameIndex(table: "dbo.Subjects", name: "IX_Category_Id", newName: "IX_CategoryId");
            RenameIndex(table: "dbo.Friends", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.News", name: "IX_User_id_Id", newName: "IX_UserId");
            AddColumn("dbo.Friends", "FriendId", c => c.String());
            AlterColumn("dbo.Posts", "ThreadId", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "ThreadId");
            AddForeignKey("dbo.Posts", "ThreadId", "dbo.Threads", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ThreadId", "dbo.Threads");
            DropIndex("dbo.Posts", new[] { "ThreadId" });
            AlterColumn("dbo.Posts", "ThreadId", c => c.Int());
            DropColumn("dbo.Friends", "FriendId");
            RenameIndex(table: "dbo.News", name: "IX_UserId", newName: "IX_User_id_Id");
            RenameIndex(table: "dbo.Friends", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Subjects", name: "IX_CategoryId", newName: "IX_Category_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_ThreadId", newName: "IX_Post_Id");
            RenameIndex(table: "dbo.Threads", name: "IX_SubjectId", newName: "IX_Subject_Id");
            RenameIndex(table: "dbo.Threads", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.News", name: "UserId", newName: "User_id_Id");
            RenameColumn(table: "dbo.Friends", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Threads", name: "SubjectId", newName: "Subject_Id");
            RenameColumn(table: "dbo.Photos", name: "ThreadId", newName: "Post_Id");
            RenameColumn(table: "dbo.Posts", name: "ThreadId", newName: "Thread_Id");
            RenameColumn(table: "dbo.Threads", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Subjects", name: "CategoryId", newName: "Category_Id");
            CreateIndex("dbo.Posts", "Thread_Id");
            AddForeignKey("dbo.Posts", "Thread_Id", "dbo.Threads", "Id");
        }
    }
}
