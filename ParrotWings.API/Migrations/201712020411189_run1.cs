namespace ParrotWings.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class run1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Active", c => c.Boolean(nullable: false));
        }
    }
}
