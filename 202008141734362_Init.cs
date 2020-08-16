namespace BelajarCRUDWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        Stock = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_m_supplier", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.tb_m_supplier",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        JoinDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.transaction_item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_m_transaction", t => t.TransactionId, cascadeDelete: true)
                .ForeignKey("dbo.item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.TransactionId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.tb_m_transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.transaction_item", "ItemId", "dbo.item");
            DropForeignKey("dbo.transaction_item", "TransactionId", "dbo.tb_m_transaction");
            DropForeignKey("dbo.item", "SupplierId", "dbo.tb_m_supplier");
            DropIndex("dbo.transaction_item", new[] { "ItemId" });
            DropIndex("dbo.transaction_item", new[] { "TransactionId" });
            DropIndex("dbo.item", new[] { "SupplierId" });
            DropTable("dbo.tb_m_transaction");
            DropTable("dbo.transaction_item");
            DropTable("dbo.tb_m_supplier");
            DropTable("dbo.item");
        }
    }
}
