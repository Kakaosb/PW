namespace ParrotWings.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "Name", c => c.String(maxLength: 55));
            DropColumn("dbo.User", "Sername");
            DropColumn("dbo.User", "Login");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Login", c => c.String(maxLength: 51));
            AddColumn("dbo.User", "Sername", c => c.String(maxLength: 30));
            AlterColumn("dbo.User", "Name", c => c.String(maxLength: 20));
        }
    }
}
