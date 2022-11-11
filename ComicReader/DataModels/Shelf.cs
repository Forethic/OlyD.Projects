using Lia.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace ComicReader.DataModels
{
    public class Shelf : LibraryItem
    {
        /// <summary>
        /// 子文件夹
        /// </summary>
        public List<Shelf> Shelves { get; set; }

        /// <summary>
        /// 当前文件夹 漫画书
        /// </summary>
        public List<Book> Books { get; set; }

        private StorageFolder _folder;

        public Shelf()
        {
        }

        public Shelf(StorageFolder folder)
        {
            _folder = folder ?? throw new ArgumentNullException("folder");

            FutureAccessListUtil.AddFolder(folder);

            Books = new List<Book>();
            Shelves = new List<Shelf>();

            Path = _folder.Path;
            Name = _folder.Name;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            var list = new List<Book>();

            list.AddRange(Books);
            foreach (var shelf in Shelves)
            {
                list.AddRange(shelf.GetAllBooks());
            }
            return list;
        }

        public Book GetFirstBook()
        {
            if (Books.Count > 0) { return Books[0]; }
            if (Shelves.Count == 0) { return null; }
            foreach (var shelf in Shelves)
            {
                var book = shelf.GetFirstBook();
                if (book == null) { continue; }
                return book;
            }
            return null;
        }

        public async Task LoadContent()
        {
            QueryOptions queryComic = new QueryOptions(CommonFileQuery.DefaultQuery, ExtensionUtil.GetComicBookExtensions());
            QueryOptions queryImage = new QueryOptions(CommonFileQuery.DefaultQuery, ExtensionUtil.GetImageExtensions());

            queryComic.FolderDepth = FolderDepth.Shallow;
            queryImage.FolderDepth = FolderDepth.Shallow;

            var folders = await _folder.GetFoldersAsync();

            for (int i = 0; i < folders.Count; i++)
            {
                var folder = folders[i];

                var innerComics = await folder.CreateFileQueryWithOptions(queryComic).GetFilesAsync(0u, 1u);
                var innerImages = await folder.CreateFileQueryWithOptions(queryImage).GetFilesAsync(0u, 1u);

                var hasComic = innerComics.Any();
                var hasImage = innerImages.Any();
                var hasFolder = (await folder.GetFoldersAsync(CommonFolderQuery.DefaultQuery, 0u, 1u)).Any();

                if (hasFolder || hasComic)
                {
                    // 找到书架
                    var shelf = new Shelf(folder);
                    await shelf.LoadContent();
                    Shelves.Add(shelf);
                }
                else if (!hasFolder && hasImage)
                {
                    // 找到漫画书
                    var book = new Book(folder.Path, folder.DisplayName);
                    Books.Add(book);
                }
            }
        }

        public async Task RemoveShelf(Shelf shelf)
        {
            if (shelf == null) { return; }
            await RemoveShelf(shelf.Id);
        }
        public async Task RemoveShelf(Guid shelfId)
        {
            if (shelfId == Guid.Empty) { return; }
            if (Shelves.Count == 0) { return; }
            var find = Shelves.Find(s => s.Id == shelfId);
            if (find == null) { return; }

            find.Dispose();
        }
        public async Task RemoveAllShelves()
        {
            if (Shelves.Count == 0) { return; }

            foreach (var shelf in Shelves.ToArray())
            {
                await RemoveShelf(shelf);
            }
            Shelves.Clear();
        }

        public async Task RemoveBook(Guid bookId)
        {
            if (bookId == Guid.Empty) { return; }
            if (Books.Count == 0) { return; }
            var find = Books.Find(b => b.Id == bookId);
            if (find == null) { return; }

            Books.Remove(find);
        }
        public async Task RemoveBook(Book book)
        {
            if (book == null) { return; }
            await RemoveBook(book.Id);
        }
        public async Task RemoveAllBooks()
        {
            if (Books.Count == 0) { return; }

            foreach (var book in Books.ToArray())
            {
                await RemoveBook(book);
            }
            Books.Clear();
        }

        public async void Dispose()
        {
            // 解除路径的使用权
            FutureAccessListUtil.RemoveFolder(_folder);

            await RemoveAllBooks();
            await RemoveAllShelves();
        }
    }
}