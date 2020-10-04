using Microsoft.AspNetCore.Http;
using System;
using user_service.Models;

namespace user_service.Service
{
    public interface IApplicationService
    {
        string StoreUploadedImage(IFormFile file);
        string StoreUploadedImage(string base64Content);
        MessageResponse GetExceptionMessage(Exception e);
    }
}
