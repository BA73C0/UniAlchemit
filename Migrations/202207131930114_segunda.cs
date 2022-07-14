namespace Pruebas2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class segunda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inscripcion",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    IdAlumno = c.Int(),
                    IDMateria = c.String(),
                    Fecha = c.String(),
                    Nota = c.Int(),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Inscripcion");
        }
    }
}
