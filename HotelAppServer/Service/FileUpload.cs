using HotelAppServer.Service.IService;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HotelAppServer.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FileUpload(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }
        public bool DeleteFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, "RoomImages", fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(file.Name);
                var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                var fileDirectory = $"{webHostEnvironment.WebRootPath}\\RoomImages";
                var filePath = Path.Combine(fileDirectory, fileName);

                var memoryStream = new MemoryStream();
                await file.OpenReadStream(maxAllowedSize: 3 * 1024 * 1024).CopyToAsync(memoryStream);

                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);

                await using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }
                var url = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}/";
                var fullPath = $"{url}RoomImages/{fileName}";
                return fullPath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
