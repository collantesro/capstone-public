using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives; // For Query below
using System.IO; // For MemoryStream
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.Common; // To generate the barcode

// Kinda following from: https://www.paddo.org/asp-net-core-image-resizing-middleware/

namespace CCSInventory.Middleware
{
    /// <summary>
    /// This custom middleware uses the ZXing library to generate a barcode bitmap,
    /// then it uses SixColors.ImageSharp to convert the bitmap into PNG.
    /// </summary>
    public class BarcodeMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<BarcodeMiddleware> _log;

        public BarcodeMiddleware(RequestDelegate next, IHostingEnvironment env, ILogger<BarcodeMiddleware> log)
        {
            _next = next;
            _env = env;
            _log = log;
        }

        /// <summary>
        /// This is the primary method invoked by the framework to process middleware.
        /// This method calls the next one in the chain if it's not going to handle it.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            // Passes along the request if the path isn't /barcode
            // and if ?code= isn't specified in the query string.
            if (path == null || !path.HasValue || !path.Value.StartsWith("/barcode") ||
                context.Request.Query["code"] == StringValues.Empty)
            {
                await _next.Invoke(context);
                return;
            }
            else
            {
                string content = context.Request.Query["code"].ToString();
                _log.LogInformation("Generating barcode for data: " + content);

                // This is the ZXing barcode writer for Code39 barcodes.
                var barcodeWriter = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_39,
                    Options = new EncodingOptions
                    {
                        Height = 250,
                        Width = 500,
                    }
                };

                // pixelData is in format Bgra32
                var pixelData = barcodeWriter.Write(content);

                // This is to convert to PNG
                using (MemoryStream pngStream = new MemoryStream())
                using (Image<Bgra32> bitmap = Image.LoadPixelData<Bgra32>(pixelData.Pixels, pixelData.Width, pixelData.Height))
                {
                    // Encoding options for PNG: Grayscale 8bit depth (8bit is lowest right now)
                    // This should make the barcode image smaller.
                    PngEncoder encoder = new PngEncoder();
                    encoder.CompressionLevel = 9;
                    encoder.ColorType = PngColorType.Grayscale;
                    encoder.BitDepth = PngBitDepth.Bit8;

                    bitmap.Save(pngStream, encoder);

                    pngStream.Seek(0, SeekOrigin.Begin);
                    context.Response.ContentType = "image/png";
                    context.Response.ContentLength = (int)pngStream.Length;
                    await context.Response.Body.WriteAsync(pngStream.GetBuffer(), 0, (int)pngStream.Length);
                }
                return;
            }
        }
    }

    /// <summary>
    /// This class contains an extension method to allow app.UseBarcodeGenerator() in Startup.cs
    /// </summary>
    public static class BarcodeMiddlewareExtensions
    {
        /// <summary>
        /// Extension method for UseBarcodeGenerator()
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseBarcodeGenerator(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BarcodeMiddleware>();
        }
    }
}
