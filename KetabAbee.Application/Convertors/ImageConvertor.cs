using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.Convertors
{
    public class ImageConvertor
    {
        public void Image_resize(string inputImagePath, string outputImagePath, int newWidth)
        {
            const long quality = 50L;

            var sourceBitmap = new Bitmap(inputImagePath);

            double dblWidthOriginal = sourceBitmap.Width;

            double dblHeightOriginal = sourceBitmap.Height;

            var relHeightWidth = dblHeightOriginal / dblWidthOriginal;

            var newHeight = (int)(newWidth * relHeightWidth);

            //< create Empty DrawArea >

            var newDrawArea = new Bitmap(newWidth, newHeight);

            //</ create Empty DrawArea >

            using (var graphicOfDrawArea = Graphics.FromImage(newDrawArea))
            {
                //< setup >

                graphicOfDrawArea.CompositingQuality = CompositingQuality.HighSpeed;

                graphicOfDrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphicOfDrawArea.CompositingMode = CompositingMode.SourceCopy;

                //</ setup >

                //< draw into placeholder >

                //*imports the image into the drawArea

                graphicOfDrawArea.DrawImage(sourceBitmap, 0, 0, newWidth, newHeight);

                //</ draw into placeholder >

                //--< Output as .Jpg >--

                using (var output = File.Open(outputImagePath, FileMode.Create))
                {
                    //< setup jpg >

                    var qualityParamId = System.Drawing.Imaging.Encoder.Quality;

                    var encoderParameters = new EncoderParameters(1);

                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);

                    //</ setup jpg >

                    //< save Bitmap as Jpg >

                    var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    if (codec != null) newDrawArea.Save(output, codec, encoderParameters);

                    //resized_Bitmap.Dispose ();

                    output.Close();

                    //</ save Bitmap as Jpg >
                }

                //--</ Output as .Jpg >--

                graphicOfDrawArea.Dispose();

            }

            sourceBitmap.Dispose();
        }
    }
}
