namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pinningThreads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Threads", "IsPinned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Threads", "IsPinned");
        }
    }
}
