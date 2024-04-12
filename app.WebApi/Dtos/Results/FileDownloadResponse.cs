namespace app.WebApi.Dtos.Results
{
    public class FileDownloadResponse
    {
        /// <summary>
        /// The fileStream/Blob
        /// </summary>
        public Stream File { get; set; }

        /// <summary>
        /// The filename and the extension
        /// </summary>
        /// <example>image.png</example>
        public string Name { get; set; }

        /// <summary>
        /// Type of data the file contains
        /// </summary>
        public string MimeType { get; set; }
    }
}