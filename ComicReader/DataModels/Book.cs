using Lia.Extensions;
using System;

namespace ComicReader.DataModels
{
    public class Book : LibraryItem
    {
        public string FriendlyName { get; set; }

        public bool HasThumbnail { get; set; }

        public DateTimeOffset CreatedTime { get; set; }

        public Book()
        {
        }

        public Book(string fullpath, string friendlyName = null)
        {
            Path = fullpath;
            Name = Path.GetFileName();
            FriendlyName = friendlyName ?? System.IO.Path.GetFileNameWithoutExtension(fullpath);

            CreatedTime = DateTimeOffset.UtcNow;
        }
    }
}