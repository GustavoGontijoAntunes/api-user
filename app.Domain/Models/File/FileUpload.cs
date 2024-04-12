using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace app.Domain.Models.File
{
    public class FileUpload
    {
        /// <summary>
        /// A file to upload to storage
        /// </summary>
        [Required]
        public IFormFile FormFile { get; set; }

        /// <summary>
        /// The subfolder to insert the file
        /// </summary>
        /// <example>public</example>
        public string SubFolder { get; set; }

        /// <summary>
        /// The file's custom name
        /// </summary>
        /// <example>public</example>
        public string CustomName { get; set; }

        public Stream OpenFileStream()
        {
            return FormFile.OpenReadStream();
        }

        public string GetContentType()
        {
            return FormFile.ContentType;
        }

        public string GetFileName()
        {
            return CustomName ?? FormFile.FileName;
        }

        public string GetPathUpload()
        {
            if (string.IsNullOrWhiteSpace(SubFolder))
            {
                return GetFileName();
            }

            return $"{SubFolder}/{Path.GetFileName(GetFileName())}";
        }
    }
}