using HotelAppServer.Service.IService;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HotelAppServer.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
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
                await file.OpenReadStream(maxAllowedSize:3*1024*1024).CopyToAsync(memoryStream);

                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);

                await using (var fs = new FileStream(filePath,FileMode.Create,FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }
                var fullPath = $"RoomImages/{fileName}";
                return fullPath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
