namespace ParrotWings.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixeddiffered : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.SenderId)
                .ForeignKey("dbo.User", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.SenderId)
                .Index(t => t.RecipientId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        Sername = c.String(maxLength: 30),
                        Login = c.String(maxLength: 51),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransactionLog", "RecipientId", "dbo.User");
            DropForeignKey("dbo.TransactionLog", "SenderId", "dbo.User");
            DropIndex("dbo.TransactionLog", new[] { "RecipientId" });
            DropIndex("dbo.TransactionLog", new[] { "SenderId" });
            DropTable("dbo.User");
            DropTable("dbo.TransactionLog");
        }
    }
}
