using System;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Lia.Utils
{
    public enum ImageSize
    {
        Small,
        Wide,
        Large,
    }

    public static class ImageUtil
    {
        public static ResolutionScale ResolutionScale { get; set; }

        public static async Task<InMemoryRandomAccessStream> GenerateImage(InMemoryRandomAccessStream stream, ImageSize size)
        {
            GetThumbnailDimensions(size, out uint width, out uint height);
            return await ResizeImage(stream, width, height);
        }

        public static async Task<InMemoryRandomAccessStream> ResizeImage(InMemoryRandomAccessStream stream, uint? width, uint? height)
        {
            try
            {
                var fileStream = new InMemoryRandomAccessStream();
                stream.Seek(0u);
                var decoder = await BitmapDecoder.CreateAsync(stream);

                uint scaledWidth, scaledHeight;
                if (width.HasValue && height.HasValue)
                {
                    if (width.Value > decoder.PixelWidth || height.Value > decoder.PixelHeight) { return null; }

                    scaledWidth = width.Value;
                    scaledHeight = height.Value;
                }
                else if (width.HasValue)
                {
                    if (width.Value > decoder.PixelWidth) { return null; }

                    scaledWidth = width.Value;
                    decimal scale = (decimal)decoder.PixelWidth / decoder.PixelWidth;
                    scaledHeight = (uint)(scaledWidth / scale);
                }
                else if (height.HasValue)
                {
                    if (height.Value > decoder.PixelHeight) { return null; }

                    scaledHeight = height.Value;
                    decimal scale = (decimal)decoder.PixelHeight / decoder.PixelWidth;
                    scaledWidth = (uint)(scaledHeight / scale);
                }
                else
                {
                    throw new ArgumentException("Either width or height must be provided (or both).");
                }

                var transform = new BitmapTransform();
                transform.InterpolationMode = BitmapInterpolationMode.Fant;
                transform.ScaledWidth = scaledWidth;
                transform.ScaledHeight = scaledHeight;

                var provider = await decoder.GetPixelDataAsync(BitmapPixelFormat.Unknown, BitmapAlphaMode.Ignore, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);
                encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Ignore, scaledWidth, scaledHeight, decoder.DpiX, decoder.DpiY, provider.DetachPixelData());
                await encoder.FlushAsync();
                return fileStream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void GetThumbnailDimensions(ImageSize size, out uint width, out uint height)
        {
            var scale = 1.0;
            width = size == ImageSize.Small ? 160u : 330u;
            height = size == ImageSize.Small ? 230u : 470u;
            var resolutionScale = ResolutionScale;

            var resolutionScaleInt = (int)resolutionScale;
            scale = resolutionScaleInt * 0.01;

            width = (uint)(scale * width);
            height = (uint)(scale * height);
        }
    }
}
