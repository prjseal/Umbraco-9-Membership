using Microsoft.AspNetCore.Http;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace Umbraco9Membership.Services
{
    public class MediaUploadService : IMediaUploadService
    {
        private readonly IMediaService _mediaService;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
        private readonly MediaFileManager _mediaFileManager;
        private readonly MediaUrlGeneratorCollection _mediaUrlGeneratorCollection;
        private readonly IShortStringHelper _shortStringHelper;

        public MediaUploadService(IMediaService mediaService,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
            MediaFileManager mediaFileManager,
            MediaUrlGeneratorCollection mediaUrlGeneratorCollection,
            IShortStringHelper shortStringHelper)
        {
            _mediaService = mediaService;
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
            _mediaFileManager = mediaFileManager;
            _mediaUrlGeneratorCollection = mediaUrlGeneratorCollection;
            _shortStringHelper = shortStringHelper;
        }

        /// <summary>
        /// You pass in a file and it uploads it to the media section and returns the udi for the media item
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="parentId">The id of the parent media item</param>
        /// <param name="mediaTypeAlias">The media type alias, e.g. Image, File, Folder</param>
        /// <returns>The udi for the new media item</returns>
        public string CreateMediaItemFromFileUpload(IFormFile file, int parentId, string mediaTypeAlias, int userId = -1, bool returnUdi = true)
        {
            var mediaItem = _mediaService.CreateMedia(file.FileName.ToFriendlyName(), parentId, mediaTypeAlias);
            return SetMediaItemValueFromFileUpload(file, mediaItem, returnUdi);
        }

        /// <summary>
        /// You pass in a file and it uploads it to the media section and returns the udi for the media item
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="parentId">The id of the parent media item to upload it to</param>
        /// <param name="mediaTypeAlias">The media type alias, e.g. Image, File, Folder</param>
        /// <returns>The udi for the new media item</returns>
        public string CreateMediaItemFromFileUpload(IFormFile file, Guid parentId, string mediaTypeAlias, int userId = -1, bool returnUdi = true)
        {
            var mediaItem = _mediaService.CreateMedia(file.FileName.ToFriendlyName(), parentId, mediaTypeAlias);
            return SetMediaItemValueFromFileUpload(file, mediaItem, returnUdi);
        }

        /// <summary>
        /// You pass in a file and it uploads it to the media section and returns the udi for the media item
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="parent">The parent media item</param>
        /// <param name="mediaTypeAlias">The media type alias, e.g. Image, File, Folder</param>
        /// <returns>The udi for the new media item</returns>
        public string CreateMediaItemFromFileUpload(IFormFile file, IMedia parent, string mediaTypeAlias, int userId = -1, bool returnUdi = true)
        {
            var mediaItem = _mediaService.CreateMedia(file.FileName.ToFriendlyName(), parent, Constants.Conventions.MediaTypes.Image);
            return SetMediaItemValueFromFileUpload(file, mediaItem, returnUdi);
        }

        /// <summary>
        /// You pass in a file and it uploads it to the media item
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="mediaItem">The media item to upload the file to</param>
        /// <returns>The udi for the new media item</returns>
        public string SetMediaItemValueFromFileUpload(IFormFile file, IMedia mediaItem, bool returnUdi = true)
        {
            mediaItem.SetValue(_mediaFileManager, _mediaUrlGeneratorCollection, _shortStringHelper, _contentTypeBaseServiceProvider, Constants.Conventions.Media.File, file.FileName, file.OpenReadStream());

            _mediaService.Save(mediaItem);

            var udi = mediaItem.GetUdi();

            if(returnUdi)
            {
                return udi.ToString();
            }
            else
            {
                return mediaItem.Key.ToString();
            }

        }
    }
}
