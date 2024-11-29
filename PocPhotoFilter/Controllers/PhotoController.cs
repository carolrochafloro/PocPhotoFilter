using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocPhotoFilter.Contracts;
using PocPhotoFilter.Contracts.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace PocPhotoFilter.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PhotoController : ControllerBase


{

    private readonly IFilter _filter;

    public PhotoController(IFilter filter)
    {
        _filter = filter;
    }

    [HttpPost]
    public async Task<IActionResult> UploadPhotos([FromForm] ImgUploadRequest request)
    {
        if (request == null || !request.Photos.Any())
        {
            return BadRequest();
        }


        var tempDirectory = Path.Combine(Path.GetTempPath(), "ProcessedImages");
        Directory.CreateDirectory(tempDirectory);
        var filePaths = new List<string>();

        await Task.Run(() =>
        {
            Parallel.ForEach(request.Photos, photo =>
            {
                using (var stream = new MemoryStream())
                {
                    photo.CopyTo(stream);
                    var image = Image.FromStream(stream);
                    var processedImage = _filter.ApplyFilter(image);

                    var fileName = Path.Combine(tempDirectory, $"{Path.GetFileNameWithoutExtension(photo.FileName)}_processed.jpg");
                    processedImage.Save(fileName, ImageFormat.Jpeg);
                    filePaths.Add(fileName);
                }
            });

        });

        return Ok(filePaths);
    }

    [HttpGet]
    public IActionResult GetPhoto(string fileName)
    {
        var filePath = Path.Combine(Path.GetTempPath(), "ProcessedImages", fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        return PhysicalFile(filePath, "image/jpeg", fileName);
    }

}
