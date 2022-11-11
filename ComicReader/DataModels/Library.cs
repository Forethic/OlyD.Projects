using ComicReader.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ComicReader.DataModels
{
    public class Library
    {
        public static Library Instance => _instance ?? (_instance = new Library());
        private static Library _instance;

        public event EventHandler BeginRefresh;
        public event EventHandler EndRefresh;

        public List<Shelf> Shelves { get; private set; } = new List<Shelf>();

        private Library()
        {
        }

        public async Task AddFolderToLibrary(StorageFolder folder)
        {
            // 判断是否是 添加过 或者 是已添加文件夹的子文件夹
            foreach (var shelf in Shelves)
            {
                if (IsFolderInFolder(folder.Path, shelf.Path))
                {
                    throw new FolderAlreadyPresentException();
                }
            }

            // 判断是否是 已添加文件夹的母文件夹
            var waitForRemove = new List<Shelf>();
            foreach (var shelf in Shelves)
            {
                if (IsFolderInFolder(shelf.Path, folder.Path))
                {
                    waitForRemove.Add(shelf);
                }
            }

            foreach (var shelf in Shelves)
            {
                RemoveShelf(shelf);
            }
            waitForRemove.Clear();
            waitForRemove = null;

            Shelves.Add(new Shelf(folder));

            // 刷新书架
            await Refresh();
        }

        /// <summary>
        /// 刷新书库
        /// </summary>
        public async Task Refresh()
        {
            BeginRefresh?.Invoke(this, new EventArgs());
            await Task.Run(async () =>
            {
                foreach (var shelf in Shelves)
                {
                    await shelf.LoadContent();
                }
            });
            EndRefresh?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 移除书架
        /// </summary>
        public void RemoveShelf(Shelf shelf)
        {
            if (shelf == null) { return; }
            RemoveShelf(shelf.Id);
        }

        /// <summary>
        /// 移除书架
        /// </summary>
        public void RemoveShelf(Guid shelfID)
        {
            if (shelfID == Guid.Empty) { return; }
            if (Shelves == null || Shelves.Count == 0) { return; }

            var find = Shelves.Find(s => s.Id == shelfID);
            if (find == null) { return; }

            find.Dispose();
            Shelves.Remove(find);
        }

        /// <summary>
        /// 判断 folder1 是否是 folder2的本体或者子文件夹
        /// </summary>
        private static bool IsFolderInFolder(string folder1, string folder2)
        {
            ValidatePath(ref folder1);
            ValidatePath(ref folder2);

            return folder1.StartsWith(folder2);
        }

        private static void ValidatePath(ref string folder1)
        {
            if (string.IsNullOrEmpty(folder1)) { return; }

            folder1 += folder1.EndsWith("\\") ? "" : "\\";
        }
    }
}