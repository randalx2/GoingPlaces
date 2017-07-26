namespace GoingPlaces.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        Description1 = c.String(),
                        Description2 = c.String(),
                        Description3 = c.String(),
                        Image1 = c.Binary(),
                        Image2 = c.Binary(),
                        Image3 = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "LocationId", "dbo.Locations");
            DropIndex("dbo.Images", new[] { "LocationId" });
            DropTable("dbo.Locations");
            DropTable("dbo.Images");
        }
    }
}
