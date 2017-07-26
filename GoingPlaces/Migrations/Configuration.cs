namespace GoingPlaces.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using GoingPlaces.Models;
    using System.Drawing;
    using System.IO;

    internal sealed class Configuration : DbMigrationsConfiguration<GoingPlaces.Models.GoingPlacesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        //Method for seeding image Batman-Jim-Lee.jpg

        public byte[] ImageToArray()
        {
            Image img = Image.FromFile(@"C:\Batman-Jim-Lee.jpg");
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            return arr;
        }

        protected override void Seed(GoingPlaces.Models.GoingPlacesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Locations.AddOrUpdate(x => x.Id,
                new Location() { Id = 1, Name = "Gotham City", Latitude = 43.899, Longitude = 89.099 },
                new Location() { Id = 2, Name = "Metropolis", Latitude = 23.994, Longitude = 87.343 },
                new Location() { Id = 3, Name = "Starling City", Latitude = 56.343, Longitude = 34.123 },
                new Location() { Id = 4, Name = "Central City", Latitude = 56.266, Longitude = 24.456 }
                );

            context.Images.AddOrUpdate(x => x.Id,
                new Images() { Id = 1, LocationId = 1, Description1 = "Could not find Image Data", Description2 = "Could not find Image Data", Description3 = "Could not find Image Data", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 2, LocationId = 1, Description1 = "Could not find Image Data", Description2 = "Could not find Image Data", Description3 = "Could not find Image Data", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 3, LocationId = 2, Description1 = "Could not find Image Data", Description2 = "Could not find Image Data", Description3 = "Could not find Image Data", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 4, LocationId = 2, Description1 = "Could not find Image Data", Description2 = "Could not find Image Data", Description3 = "Could not find Image Data", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 5, LocationId = 3, Description1 = "Could not find Image Data", Description2 = "Could not find Image Data", Description3 = "Could not find Image Data", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 6, LocationId = 3, Description1 = "Could not find Image Data", Description2 = "Could not find Image Data", Description3 = "Could not find Image Data", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 7, LocationId = 4, Description1 = "Could not find Image Data", Description2 = "Could not find Image Data", Description3 = "Could not find Image Data", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() }
                );

        }
    }
}
