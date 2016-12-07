namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moderators : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moderators", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Moderators", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.Moderators", new[] { "SubjectId" });
            DropIndex("dbo.Moderators", new[] { "UserId" });
            DropTable("dbo.Moderators");
        }
    }
}
