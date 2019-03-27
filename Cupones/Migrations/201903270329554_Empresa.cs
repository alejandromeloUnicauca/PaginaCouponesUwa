namespace Cupones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Empresa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Empresa",
                c => new
                    {
                        idEmpresa = c.Int(nullable: false, identity: true),
                        nit = c.String(),
                        nombre = c.String(),
                        telefono = c.String(),
                    })
                .PrimaryKey(t => t.idEmpresa);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Empresa");
        }
    }
}
