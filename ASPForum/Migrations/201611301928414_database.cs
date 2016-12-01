namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class database : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.Subjects", new[] { "Category_Id" });
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropIndex("dbo.Posts", new[] { "Subject_Id" });
            DropIndex("dbo.Photos", new[] { "Post_Id" });
            DropIndex("dbo.Friends", new[] { "Friend_Id" });
            DropIndex("dbo.Friends", new[] { "User_Id" });
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 70),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        User_id_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_id_Id)
                .Index(t => t.User_id_Id);
            
            AlterColumn("dbo.Categories", "Title", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Subjects", "Title", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Subjects", "Category_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "Title", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Posts", "User_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Posts", "Subject_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "Title", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Messages", "Title", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Photos", "Title", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Photos", "Post_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Friends", "Friend_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Friends", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Subjects", "Category_Id");
            CreateIndex("dbo.Posts", "Subject_Id");
            CreateIndex("dbo.Posts", "User_Id");
            CreateIndex("dbo.Photos", "Post_Id");
            CreateIndex("dbo.Friends", "Friend_Id");
            CreateIndex("dbo.Friends", "User_Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.News", "User_id_Id", "dbo.AspNetUsers");
            DropIndex("dbo.News", new[] { "User_id_Id" });
            DropIndex("dbo.Friends", new[] { "User_Id" });
            DropIndex("dbo.Friends", new[] { "Friend_Id" });
            DropIndex("dbo.Photos", new[] { "Post_Id" });
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropIndex("dbo.Posts", new[] { "Subject_Id" });
            DropIndex("dbo.Subjects", new[] { "Category_Id" });
            AlterColumn("dbo.Friends", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Friends", "Friend_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Photos", "Post_Id", c => c.Int());
            AlterColumn("dbo.Photos", "Title", c => c.String());
            AlterColumn("dbo.Messages", "Title", c => c.String());
            AlterColumn("dbo.Comments", "Title", c => c.String());
            AlterColumn("dbo.Posts", "Subject_Id", c => c.Int());
            AlterColumn("dbo.Posts", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Posts", "Title", c => c.String());
            AlterColumn("dbo.Posts", "Content", c => c.String());
            AlterColumn("dbo.Subjects", "Category_Id", c => c.Int());
            AlterColumn("dbo.Subjects", "Title", c => c.String());
            AlterColumn("dbo.Categories", "Title", c => c.String());
            DropTable("dbo.News");
            CreateIndex("dbo.Friends", "User_Id");
            CreateIndex("dbo.Friends", "Friend_Id");
            CreateIndex("dbo.Photos", "Post_Id");
            CreateIndex("dbo.Posts", "Subject_Id");
            CreateIndex("dbo.Posts", "User_Id");
            CreateIndex("dbo.Subjects", "Category_Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
