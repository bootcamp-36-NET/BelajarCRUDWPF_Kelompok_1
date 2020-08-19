namespace BelajarCRUDWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsResetColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_m_supplier", "IsReset", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_m_supplier", "IsReset");
        }
    }
}
