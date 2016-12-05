namespace ASPForum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class friend : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Friends", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.Friends", name: "IX_UserId", newName: "IX_User_Id");
            DropColumn("dbo.Friends", "FriendId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friends", "FriendId", c => c.String());
            RenameIndex(table: "dbo.Friends", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Friends", name: "User_Id", newName: "UserId");
        }
    }
}
