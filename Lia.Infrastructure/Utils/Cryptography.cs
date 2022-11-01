using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Lia.Utils
{
    public class Cryptography
    {
        /// <summary>
        /// 计算字符串MD5 - 16进制
        /// </summary>
        public static string ComputeMD5(string str)
        {
            var hashAlgorithmProvider = HashAlgorithmProvider.OpenAlgorithm("MD5");
            var data = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            return CryptographicBuffer.EncodeToHexString(hashAlgorithmProvider.HashData(data));
        }
    }
}