namespace Cupones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Empresa1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupon", "idEmpresa", c => c.Int(nullable: false));
            CreateIndex("dbo.Coupon", "idEmpresa");
            AddForeignKey("dbo.Coupon", "idEmpresa", "dbo.Empresa", "idEmpresa", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coupon", "idEmpresa", "dbo.Empresa");
            DropIndex("dbo.Coupon", new[] { "idEmpresa" });
            DropColumn("dbo.Coupon", "idEmpresa");
        }
    }
}
