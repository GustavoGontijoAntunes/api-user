using app.Domain.Extensions;

namespace app.Domain.Models.File
{
    public class FileDownload
    {
        public FileDownload(Stream stream, string filename)
        {
            File = stream;
            Name = filename.GetLastName();
            MimeType = filename.GetMimeType();
        }

        /// <summary>
        /// The fileStream/Blob
        /// </summary>
        public Stream File { get; private set; }

        /// <summary>
        /// The filename and the extension
        /// </summary>
        /// <example>image.png</example>
        public string Name { get; private set; }

        /// <summary>
        /// Type of data the file contains
        /// </summary>
        public string MimeType { get; private set; }
    }
}