namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moderator : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Moderators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Subject_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moderators", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Moderators", "Subject_Id", "dbo.Subjects");
            DropIndex("dbo.Moderators", new[] { "User_Id" });
            DropIndex("dbo.Moderators", new[] { "Subject_Id" });
            DropTable("dbo.Moderators");
        }
    }
}
