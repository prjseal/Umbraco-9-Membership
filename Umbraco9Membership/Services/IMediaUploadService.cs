using Microsoft.AspNetCore.Http;
using System;
using Umbraco.Cms.Core.Models;

namespace Umbraco9Membership.Services
{
    public interface IMediaUploadService
    {
        string CreateMediaItemFromFileUpload(IFormFile file, int parentId, string mediaTypeAlias,
            int userId = -1, bool returnUdi = true);

        string CreateMediaItemFromFileUpload(IFormFile file, Guid parentId, string mediaTypeAlias,
            int userId = -1, bool returnUdi = true);

        string CreateMediaItemFromFileUpload(IFormFile file, IMedia parent, string mediaTypeAlias,
            int userId = -1, bool returnUdi = true);

        string SetMediaItemValueFromFileUpload(IFormFile file, IMedia mediaItem, bool returnUdi = true);
    }
}
