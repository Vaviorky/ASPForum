namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class database1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subjects", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Posts", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Photos", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.Friends", new[] { "User_Id" });
            AlterColumn("dbo.Friends", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Friends", "User_Id");
            AddForeignKey("dbo.Subjects", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Posts", "Subject_Id", "dbo.Subjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Photos", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Photos", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Posts", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Friends", new[] { "User_Id" });
            AlterColumn("dbo.Friends", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Friends", "User_Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Photos", "Post_Id", "dbo.Posts", "Id");
            AddForeignKey("dbo.Posts", "Subject_Id", "dbo.Subjects", "Id");
            AddForeignKey("dbo.Subjects", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
