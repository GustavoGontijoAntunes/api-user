using MimeTypes;

namespace app.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string GetMimeType(this string filename)
        {
            var extension = Path.GetExtension(filename);
            return MimeTypeMap.GetMimeType(extension);
        }

        public static string GetLastName(this string filenameUri)
        {
            return Path.GetFileName(filenameUri);
        }

        public static bool HasExtension(this string filename)
        {
            return Path.HasExtension(filename);
        }
    }
}