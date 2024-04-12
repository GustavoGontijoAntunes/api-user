using System.ComponentModel.DataAnnotations;

namespace app.WebApi.Dtos.Requests
{
    public class FileUploadPost
    {
        /// <summary>
        /// A file to upload to storage
        /// </summary>
        [Required]
        public IFormFile FormFile { get; set; }

        /// <summary>
        /// The bucket name of S3
        /// </summary>
        /// <example>my-bucket</example>
        [Required(AllowEmptyStrings = false)]
        public string? Bucketname { get; set; }

        /// <summary>
        /// The subfolder to insert the file
        /// </summary>
        /// <example>public</example>
        public string? SubFolder { get; set; }

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
            return FormFile.FileName;
        }

        public string GetPathUpload()
        {
            if (string.IsNullOrWhiteSpace(SubFolder))
            {
                return GetFileName();
            }

            return $"{SubFolder}/{GetFileName()}";
        }
    }
}