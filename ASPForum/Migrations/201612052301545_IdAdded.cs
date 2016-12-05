namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "Thread_Id", "dbo.Threads");
            DropForeignKey("dbo.Friends", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.News", "User_id_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "Thread_Id" });
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropIndex("dbo.Friends", new[] { "User_Id" });
            DropIndex("dbo.News", new[] { "User_id_Id" });
            RenameColumn(table: "dbo.Subjects", name: "Category_Id", newName: "CategoryId");
            RenameColumn(table: "dbo.Posts", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Threads", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Posts", name: "Thread_Id", newName: "ThreadId");
            RenameColumn(table: "dbo.Photos", name: "Post_Id", newName: "ThreadId");
            RenameColumn(table: "dbo.Threads", name: "Subject_Id", newName: "SubjectId");
            RenameColumn(table: "dbo.Friends", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.News", name: "User_id_Id", newName: "UserId");
            RenameIndex(table: "dbo.Threads", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Threads", name: "IX_Subject_Id", newName: "IX_SubjectId");
            RenameIndex(table: "dbo.Photos", name: "IX_Post_Id", newName: "IX_ThreadId");
            RenameIndex(table: "dbo.Subjects", name: "IX_Category_Id", newName: "IX_CategoryId");
            AddColumn("dbo.Friends", "FriendId", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "ThreadId", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Friends", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.News", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Posts", "UserId");
            CreateIndex("dbo.Posts", "ThreadId");
            CreateIndex("dbo.Friends", "UserId");
            CreateIndex("dbo.News", "UserId");
            AddForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Posts", "ThreadId", "dbo.Threads", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Friends", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.News", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friends", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.News", new[] { "UserId" });
            DropIndex("dbo.Friends", new[] { "UserId" });
            DropIndex("dbo.Posts", new[] { "ThreadId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            AlterColumn("dbo.News", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Friends", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Posts", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Posts", "ThreadId", c => c.Int());
            DropColumn("dbo.Friends", "FriendId");
            RenameIndex(table: "dbo.Subjects", name: "IX_CategoryId", newName: "IX_Category_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_ThreadId", newName: "IX_Post_Id");
            RenameIndex(table: "dbo.Threads", name: "IX_SubjectId", newName: "IX_Subject_Id");
            RenameIndex(table: "dbo.Threads", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.News", name: "UserId", newName: "User_id_Id");
            RenameColumn(table: "dbo.Friends", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Threads", name: "SubjectId", newName: "Subject_Id");
            RenameColumn(table: "dbo.Photos", name: "ThreadId", newName: "Post_Id");
            RenameColumn(table: "dbo.Posts", name: "ThreadId", newName: "Thread_Id");
            RenameColumn(table: "dbo.Threads", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Subjects", name: "CategoryId", newName: "Category_Id");
            CreateIndex("dbo.News", "User_id_Id");
            CreateIndex("dbo.Friends", "User_Id");
            CreateIndex("dbo.Posts", "User_Id");
            CreateIndex("dbo.Posts", "Thread_Id");
            AddForeignKey("dbo.News", "User_id_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Friends", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Posts", "Thread_Id", "dbo.Threads", "Id");
            AddForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
