using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GoingPlaces.Models;
using FlickrNet;
using System.Drawing;
using System.IO;
using System.Threading;

namespace GoingPlaces.Controllers
{
    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {
        private GoingPlacesContext db = new GoingPlacesContext();

        // GET: api/Images
        [Route("")]
        [HttpGet]
        public IQueryable<Images> GetImages()
        {
            return db.Images;
        }

        // GET: api/Images/5
        [ResponseType(typeof(Images))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetImages(int id)
        {
            Images images = await db.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }

            return Ok(images);
        }

        //Local service function to convert images to byte arrays before saving to our database
        //Pass in the large URL to set the image data
        public byte[] ImageToArray(string photoUrl)
        {
            WebClient web = new WebClient();

            //Download the image from its URL to the server
            byte[] arr = web.DownloadData(photoUrl);

            return arr;
        }

        //Use this to return a default image if no image data found on flickr
        public byte[] ImageToArrayDefault()
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

        // GET: api/Images/5
        [ResponseType(typeof(Images))]
        [Route("{name}")]
        [HttpGet]
        public IEnumerable<Images>GetImageByName(string name)
        {
            //Get the first contact in the contacts list with the specified id
            //Pass in the location name and check if the main image description contains it

            //Create the flickr objects and set it up on each call


            Flickr flickr = new Flickr();
            flickr.ApiKey = "dba11127902261afd54826b290ed3de6";
            flickr.ApiSecret = "1af450a5e13b378c";

            //Create the byte arrays to store the binary data of each object returned through the JSON

            //Set up search options object
            var options = new PhotoSearchOptions() { Tags = name, PerPage = 12, Page = 1, Extras = PhotoSearchExtras.LargeUrl | PhotoSearchExtras.Tags };

            //This return all image objects including main description, landmark id and secondary images
            Images[] ImageArray = db.Images.Where<Images>(c => (c.Description1.Contains(name) || c.Description2.Contains(name) || c.Description3.Contains(name))).ToArray();
            
            //If the location or any duplicate of it is not found in the ImageArray the array size count will be O
            //To make it easier convert this array to a list object for now
            List<Images> myImageList = ImageArray.ToList<Images>();

            //If we have 0 entries in the list corresponding to the search name then check flickr

            //Initialize
            Images[] myImageObject = new Images[4]
            {
                new Images() { Id = -1, LocationId = -1, Description1 = "Image Not Found", Description2 = "Image Not Found", Description3 = "Image Not Found", Image1 = ImageToArrayDefault(), Image2 = ImageToArrayDefault(), Image3 = ImageToArrayDefault()},
                new Images() { Id = -1, LocationId = -1, Description1 = "Image Not Found", Description2 = "Image Not Found", Description3 = "Image Not Found", Image1 = ImageToArrayDefault(), Image2 = ImageToArrayDefault(), Image3 = ImageToArrayDefault()},
                new Images() { Id = -1, LocationId = -1, Description1 = "Image Not Found", Description2 = "Image Not Found", Description3 = "Image Not Found", Image1 = ImageToArrayDefault(), Image2 = ImageToArrayDefault(), Image3 = ImageToArrayDefault()},
                new Images() { Id = -1, LocationId = -1, Description1 = "Image Not Found", Description2 = "Image Not Found", Description3 = "Image Not Found", Image1 = ImageToArrayDefault(), Image2 = ImageToArrayDefault(), Image3 = ImageToArrayDefault() },
            };


            try
            {
                if (myImageList.Count <= 0)
                {
                    //Search for photos using the search option
                    //Search tags are set for a max of 3 photos per page
                    PhotoCollection photos = flickr.PhotosSearch(options);

                    //We should at least return 3 photos based on the tag

                    //If we found some photos based on tags return at least 12
                    if (photos.Count > 0)
                    {
                        int counter = 0;
                        //3 photos per image object

                        for (int j = 0; j < 4; j++)
                        {
                            if (counter < photos.Count)
                            {
                                myImageObject[j].Description1 = "Description: " + photos[counter].Title + "\n Date Uploaded: " + photos[counter].DateUploaded +
                                                           "\n Date Taken: " + photos[counter].DateTaken + "\n Place ID: " + photos[counter].PlaceId +
                                                           "\n Latitude: " + photos[counter].Latitude + "\n Longitude: " + photos[counter].Longitude;

                                myImageObject[j].Image1 = ImageToArray(photos[counter].Small320Url);
                                ++counter;
                                if (counter >= photos.Count) break;

                                myImageObject[j].Description2 = "Description: " + photos[counter].Title + "\n Date Uploaded: " + photos[counter].DateUploaded +
                                                        "\n Date Taken: " + photos[counter].DateTaken + "\n Place ID: " + photos[counter].PlaceId +
                                                        "\n Latitude: " + photos[counter].Latitude + "\n Longitude: " + photos[counter].Longitude;

                                myImageObject[j].Image2 = ImageToArray(photos[counter].Small320Url);
                                ++counter;
                                if (counter >= photos.Count) break;

                                myImageObject[j].Description3 = "Description: " + photos[counter].Title + "\n Date Uploaded: " + photos[counter].DateUploaded +
                                                        "\n Date Taken: " + photos[counter].DateTaken + "\n Place ID: " + photos[counter].PlaceId +
                                                        "\n Latitude: " + photos[counter].Latitude + "\n Longitude: " + photos[counter].Longitude;

                                myImageObject[j].Image3 = ImageToArray(photos[counter].Small320Url);
                                ++counter;
                                if (counter >= photos.Count) break;
                            }
                            else
                            {
                                break;
                            }
                        }

                        counter = 0;

                        foreach (Images image in myImageObject)
                        {
                            /*Location location = db.Locations.Where(c => c.Name.Contains(name)).FirstOrDefault<Location>();

                            //If the location is not found
                            if(location == null)
                            {
                                Location newLocation = new Location()
                                {
                                    Name = name,
                                    Latitude = 0,
                                    Longitude = 0
                                };

                                image.LocationId = newLocation.Id;
                                db.Locations.Add(newLocation);
                                db.Images.Add(image);
                                myImageList.Add(image);                           
                            }
                            else
                            {
                                //If the location name is present in the db

                            }*/

                            //Due a bug with the navigation properties set the locationID to a default value to prototype and test
                            image.LocationId = 1;
                            db.Images.Add(image);
                            myImageList.Add(image);
                        }

                        //Convert back to an array
                        ImageArray = myImageList.ToArray<Images>();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + "Your Internet Connection may be down");
            }
            

            //Save to images table in the db
            db.SaveChanges();
            return ImageArray;
        }

        // PUT: api/Images/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutImages(int id, Images images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != images.Id)
            {
                return BadRequest();
            }

            db.Entry(images).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Images
        [ResponseType(typeof(Images))]
        [HttpPost]
        public async Task<IHttpActionResult> PostImages(Images images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Images.Add(images);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = images.Id }, images);
        }

        // DELETE: api/Images/5
        [ResponseType(typeof(Images))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteImages(int id)
        {
            Images images = await db.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }

            db.Images.Remove(images);
            await db.SaveChangesAsync();

            return Ok(images);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImagesExists(int id)
        {
            return db.Images.Count(e => e.Id == id) > 0;
        }
    }
}