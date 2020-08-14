namespace BelajarCRUDWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addItem : DbMigration
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
            
            AddColumn("dbo.tb_m_supplier", "JoinDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.item", "SupplierId", "dbo.tb_m_supplier");
            DropIndex("dbo.item", new[] { "SupplierId" });
            DropColumn("dbo.tb_m_supplier", "JoinDate");
            DropTable("dbo.item");
        }
    }
}
