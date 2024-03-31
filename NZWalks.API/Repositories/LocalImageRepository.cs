using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnviroment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDBContext nZWalksDBContext;


        public LocalImageRepository(IWebHostEnvironment webHostEnviroment, IHttpContextAccessor httpContextAccessor, NZWalksDBContext nZWalksDBContext)
        {
            this.webHostEnviroment = webHostEnviroment;
            this.httpContextAccessor = httpContextAccessor;
            this.nZWalksDBContext = nZWalksDBContext;

        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnviroment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            //upload image to local
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //E:/Images/
            //https:localhost:1234/Images/image.jpg
            //save image to db

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}" +
                $"://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}" +
                $"/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;
            //Add image to images table
            await nZWalksDBContext.Images.AddAsync(image);
            await nZWalksDBContext.SaveChangesAsync();
            return image;
        }
    }
}
