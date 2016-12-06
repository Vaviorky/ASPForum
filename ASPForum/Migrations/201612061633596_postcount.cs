namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postcount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Threads", "ViewCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Threads", "ViewCount");
        }
    }
}
