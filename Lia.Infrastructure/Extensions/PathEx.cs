using System;
using System.IO;
using System.Linq;

namespace Lia.Extensions
{
    public static class PathEx
    {
        public static bool IsValidExtension(this string extension)
        {
            if (string.IsNullOrEmpty(extension)) { return false; }
            if (extension.Length <= 1) { return false; }

            return true;
        }

        public static bool IsValidFileName(this string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return false; }
            if (fileName[0] == '.') { return false; }
            return true;
        }

        public static string GetExtension(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path can't be null or empty", path);
            }

            try
            {
                return Path.GetExtension(path);
            }
            catch (Exception)
            {
                var dotIndex = path.LastIndexOf('.');
                var anyIndex = path.LastIndexOfAny(new char[] { '\\', '/' });

                if (dotIndex <= 0 || anyIndex > dotIndex)
                {
                    return string.Empty;
                }

                return path.Substring(dotIndex + 1);
            }
        }

        public static string GetFileName(this string path, bool withExtension = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path can't be null or empty", "path");
            }

            try
            {
                return withExtension ?
                       Path.GetFileName(path) :
                       Path.GetFileNameWithoutExtension(path);
            }
            catch (Exception)
            {
                var anyIndex = path.LastIndexOfAny(new char[] { '\\', '/' });
                if (anyIndex >= 0)
                {
                    path = path.Substring(anyIndex + 1);
                }

                if (!withExtension)
                {
                    var dotIndex = path.LastIndexOf('.');
                    if (dotIndex >= 0)
                    {
                        path = path.Substring(dotIndex + 1);
                    }
                }
                return path;
            }
        }

        public static string GetFolderName(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path can't be null or empty", "path");
            }

            try
            {
                if (!path.Contains('/'))
                {
                    return Path.GetDirectoryName(path);
                }
            }
            catch (Exception)
            {
            }

            var anyIndex = path.LastIndexOfAny(new char[] { '\\', '/' });
            if (anyIndex >= 0)
            {
                path = path.Substring(0, anyIndex);
            }
            return path;
        }
    }
}