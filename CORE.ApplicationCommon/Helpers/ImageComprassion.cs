using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Drawing.Drawing2D;

namespace CORE.ApplicationCommon.Helpers
{
    public static class ImageComprassion
    {
        const int size = 150;
        const int quality = 75;

        //public static string ImageOptimizeStart(string fileName)
        //{
        //    using (var image = new Bitmap(System.Drawing.Image.FromFile(fileName)))
        //    {
        //        int width, height;
        //        if (image.Width > image.Height)
        //        {
        //            width = size;
        //            height = Convert.ToInt32(image.Height * size / (double)image.Width);
        //        }
        //        else
        //        {
        //            width = Convert.ToInt32(image.Width * size / (double)image.Height);
        //            height = size;
        //        }
        //        var resized = new Bitmap(width, height);
        //        using (var graphics = Graphics.FromImage(resized))
        //        {
        //            graphics.CompositingQuality = CompositingQuality.HighSpeed;
        //            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            graphics.CompositingMode = CompositingMode.SourceCopy;
        //            graphics.DrawImage(image, 0, 0, width, height);
        //            using (var output = File.Open(
        //                OutputPath(fileName, outputDirectory, SystemDrawing), FileMode.Create))
        //            {
        //                var qualityParamId = Encoder.Quality;
        //                var encoderParameters = new EncoderParameters(1);
        //                encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
        //                var codec = ImageCodecInfo.GetImageDecoders()
        //                    .FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
        //                resized.Save(output, codec, encoderParameters);
        //            }
        //        }
        //    }
        //}
    }
}
