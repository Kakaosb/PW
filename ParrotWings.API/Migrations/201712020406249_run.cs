namespace ParrotWings.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class run : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionLog", "TotalSender", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TransactionLog", "TotalRecipient", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransactionLog", "TotalRecipient");
            DropColumn("dbo.TransactionLog", "TotalSender");
        }
    }
}
