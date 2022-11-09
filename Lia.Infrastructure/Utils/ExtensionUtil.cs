using Lia.Extensions;
using System.Collections.Generic;

namespace Lia.Utils
{
    public static class ExtensionUtil
    {
        private static string _comicBookExtensions = ".cbr;.cbz;.rar;.zip;.pdf;.epub;.7z;.cb7;.tar;.cbt";
        private static string _imageExtensions = ".jpg;.jpeg;.png;.gif;.bmp;.webp";

        public static bool IsImage(string path)
        {
            if (string.IsNullOrEmpty(path)) { return false; }

            var extension = path.GetExtension();
            var fileName = path.GetFileName();

            if (extension.IsValidExtension() && fileName.IsValidFileName())
            {
                extension = extension.ToLower();
                return _imageExtensions.Contains(extension);
            }
            return false;
        }

        public static bool IsComicBook(string path)
        {
            if (string.IsNullOrEmpty(path)) { return false; }

            var extension = path.GetExtension();
            var fileName = path.GetFileName();

            if (extension.IsValidExtension() && fileName.IsValidFileName())
            {
                extension = extension.ToLower();
                return _comicBookExtensions.Contains(extension);
            }
            return false;
        }

        public static string[] GetComicBookExtensions()
            => _comicBookExtensions.Split(';', System.StringSplitOptions.RemoveEmptyEntries);

        public static string[] GetImageExtensions()
            => _imageExtensions.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
    }
}