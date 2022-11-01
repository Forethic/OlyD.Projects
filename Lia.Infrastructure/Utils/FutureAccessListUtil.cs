using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace Lia.Utils
{
    public class FutureAccessListUtil
    {
        private static object _lock = new object();

        public static void RemoveFolder(StorageFolder storageFolder)
        {
            if (storageFolder == null) { return; }

            lock (_lock)
            {
                var folderToken = GetFolderToken(storageFolder.Path);
                if (string.IsNullOrWhiteSpace(folderToken)) { return; }

                StorageApplicationPermissions.FutureAccessList.Remove(folderToken);
            }
        }

        public static async Task RemoveFolder(string folderPath)
            => RemoveFolder(await StorageFolder.GetFolderFromPathAsync(folderPath));

        public static string AddFolder(StorageFolder storageFolder)
        {
            lock (_lock)
            {
                var folderToken = GetFolderToken(storageFolder.Path);
                if (string.IsNullOrWhiteSpace(folderToken)) { return ""; }

                StorageApplicationPermissions.FutureAccessList.AddOrReplace(folderToken, storageFolder);

                return folderToken;
            }
        }

        public static async Task<StorageFolder> GetFolder(string path, bool isFolder)
        {
            if (!isFolder)
            {
                path = Path.GetDirectoryName(path);
            }

            var folderToken = GetFolderToken(path);
            if (string.IsNullOrEmpty(folderToken)) { return null; }
            return await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
        }

        public static void Clear()
        {
            lock (_lock)
            {
                StorageApplicationPermissions.FutureAccessList.Clear();
            }
        }

        private static string GetFolderToken(string path)
            => string.IsNullOrWhiteSpace(path) ? "" : Cryptography.ComputeMD5(path);
    }
}