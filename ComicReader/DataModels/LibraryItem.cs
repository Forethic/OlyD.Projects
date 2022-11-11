using System;

namespace ComicReader.DataModels
{
    public class LibraryItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public LibraryItem()
        {
            Id = Guid.NewGuid();
        }
    }
}