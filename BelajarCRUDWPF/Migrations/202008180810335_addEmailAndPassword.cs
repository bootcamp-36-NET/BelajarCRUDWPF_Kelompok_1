namespace BelajarCRUDWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmailAndPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_m_supplier", "Email", c => c.String());
            AddColumn("dbo.tb_m_supplier", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_m_supplier", "Password");
            DropColumn("dbo.tb_m_supplier", "Email");
        }
    }
}
