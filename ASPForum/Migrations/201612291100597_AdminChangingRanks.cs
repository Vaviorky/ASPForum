namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminChangingRanks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IfAdminChangedRank", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IfAdminChangedRank");
        }
    }
}
