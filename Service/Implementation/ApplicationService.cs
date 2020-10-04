using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Linq;
using System.Text;
using user_service.Models;

namespace user_service.Service.Implementation
{
    public class ApplicationService : IApplicationService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ApplicationService> _logger;
        private readonly string dirPath;

        public ApplicationService(
            IWebHostEnvironment webhostingEnvironment,
            ILogger<ApplicationService> logger)
        {
            _webHostEnvironment = webhostingEnvironment;
            _logger = logger;
            dirPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            ValidatePathExists(dirPath);
        }

        public MessageResponse GetExceptionMessage(Exception e)
        {
            if (e.GetType().IsSubclassOf(typeof(ApplicationException)) || e is ApplicationException)
            {
                return new MessageResponse
                {
                    Message = ((ApplicationException)e).Message
                };
            }
            else if (e is MySqlException mySqlException)
            {
                return new MessageResponse
                {
                    Message = mySqlException.Message
                };
            }

            return new MessageResponse
            {
                Message = "Internal server error"
            };
        }

        public string StoreUploadedImage(string base64Content)
        {
            try
            {
                if (string.IsNullOrEmpty(base64Content))
                {
                    return null;
                }

                byte[] fileContent = Convert.FromBase64String(base64Content);
                string fileName = Path.Combine(dirPath, $"{Guid.NewGuid()}.{GetImageFormat(fileContent)}");
                File.WriteAllBytes(fileName, fileContent);
                return fileName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to save image {base64Content}");
            }

            return null;
        }

        public string StoreUploadedImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return null;
                }

                MemoryStream stream = new MemoryStream();
                file.CopyTo(stream);
                string extension = GetImageFormat(stream.GetBuffer())?.ToString();

                if (extension == null)
                {
                    throw new InvalidOperationException($"File {file.FileName} sent is not of type imge");
                }

                string fileName = $"{Guid.NewGuid()}.{extension}";
                using var fileStream = new FileStream(Path.Combine(dirPath, fileName), FileMode.Create);
                file.CopyTo(fileStream);
                fileStream.Close();
                return fileName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to save image {file.FileName}");
            }

            return null;
        }

        private void ValidatePathExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private Constant.ImageFormat? GetImageFormat(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return Constant.ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return Constant.ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return Constant.ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return Constant.ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return Constant.ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return Constant.ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return Constant.ImageFormat.jpeg;

            return null;
        }
    }
}
