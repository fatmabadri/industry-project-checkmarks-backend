using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CheckmarksWebApi.ViewModels
{
    public class FileUploadForm
    {
        public IFormFile FileToUpload {get;set;}
    }
}