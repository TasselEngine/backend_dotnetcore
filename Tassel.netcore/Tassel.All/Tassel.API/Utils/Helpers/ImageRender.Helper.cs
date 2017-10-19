using BWS.Utils.NetCore.Format;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tassel.API.Utils.Helpers {

    public class ImageRender {

        public static (string img, string thumb) ImageMutiCompress(string base64ImageString, double percent, double thumb_pct) {
            return ImageCompress(Convert.FromBase64String(base64ImageString), percent, thumb_pct);
        }

        public static string ImageCompress(string base64ImageString, double percent) {
            var (img,_) = ImageCompress(Convert.FromBase64String(base64ImageString), percent, null);
            return img;
        }

        public static (string img, string thumb) ImageCompress(byte[] base64Image, double percent, double? thumb_pct = null) {
            try {
                using (var bitmap = SKBitmap.Decode(base64Image)) {
                    if (bitmap == null)
                        return (null, null);
                    var image_ecd = default(string);
                    var image_thumb = default(string);
                    using (var mstream = new SKDynamicMemoryWStream())  // Compress Image
                        if (bitmap.Encode(mstream, SKEncodedImageFormat.Jpeg, (int)(100 * (percent > 1 ? 1 : percent))))
                            StreamToBase64String(mstream);
                    if (thumb_pct == null)
                        return (image_ecd, null);
                    using (var mstream = new SKDynamicMemoryWStream())  // Create Thumb
                        if (bitmap.Encode(mstream, SKEncodedImageFormat.Jpeg, (int)(100 * (thumb_pct > percent ? percent : thumb_pct))))
                            image_thumb = StreamToBase64String(mstream);
                    return (image_ecd, image_thumb);
                }
            } catch {
                return (null, null);
            }
        }

        private static string StreamToBase64String(SKDynamicMemoryWStream mstream) {
            var str = mstream.DetachAsStream();
            var bts = new byte[str.Length];
            str.Read(bts, str.Length);
            return Convert.ToBase64String(bts);
        }

    }

}
