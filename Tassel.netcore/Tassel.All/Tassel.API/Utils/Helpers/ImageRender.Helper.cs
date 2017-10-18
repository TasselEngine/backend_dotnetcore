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

        public static string ImageCompress(string base64ImageString, double percent, double? originWidth = null, double? originHeight = null) {
            return ImageCompress(Convert.FromBase64String(base64ImageString), percent, originWidth, originHeight);
        }

        public static string ImageCompress(byte[] base64Image, double percent, double? originWidth = null, double? originHeight = null) {
            try {
                using (var bitmap = SKBitmap.Decode(base64Image)) {
                    if (bitmap == null)
                        return null;
                    using (var mstream = new SKDynamicMemoryWStream()) {
                        System.Diagnostics.Debug.WriteLine(123456);
                        var succeed = bitmap.Encode(mstream, SKEncodedImageFormat.Jpeg, (int)(100 * (percent > 1 ? 1 : percent)));
                        if (succeed) {
                            System.Diagnostics.Debug.WriteLine(666666);
                            var str = mstream.DetachAsStream();
                            var bts = new byte[str.Length];
                            str.Read(bts, str.Length);
                            return Convert.ToBase64String(bts);
                        } else {
                            return null;
                        }
                    }
                }
            } catch {
                return null;
            }
        }


    }

}
