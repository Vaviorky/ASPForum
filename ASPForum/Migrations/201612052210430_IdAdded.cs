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
            RenameColumn(table: "dbo.Threads", name: "Subject_Id", newName: "SubjectId");
            RenameIndex(table: "dbo.Posts", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Threads", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Threads", name: "IX_Subject_Id", newName: "IX_SubjectId");
            RenameIndex(table: "dbo.Subjects", name: "IX_Category_Id", newName: "IX_CategoryId");
            AlterColumn("dbo.Posts", "ThreadId", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "ThreadId");
            AddForeignKey("dbo.Posts", "ThreadId", "dbo.Threads", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ThreadId", "dbo.Threads");
            DropIndex("dbo.Posts", new[] { "ThreadId" });
            AlterColumn("dbo.Posts", "ThreadId", c => c.Int());
            RenameIndex(table: "dbo.Subjects", name: "IX_CategoryId", newName: "IX_Category_Id");
            RenameIndex(table: "dbo.Threads", name: "IX_SubjectId", newName: "IX_Subject_Id");
            RenameIndex(table: "dbo.Threads", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Threads", name: "SubjectId", newName: "Subject_Id");
            RenameColumn(table: "dbo.Posts", name: "ThreadId", newName: "Thread_Id");
            RenameColumn(table: "dbo.Threads", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Subjects", name: "CategoryId", newName: "Category_Id");
            CreateIndex("dbo.Posts", "Thread_Id");
            AddForeignKey("dbo.Posts", "Thread_Id", "dbo.Threads", "Id");
        }
    }
}
