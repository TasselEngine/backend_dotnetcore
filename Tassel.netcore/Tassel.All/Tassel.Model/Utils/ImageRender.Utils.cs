using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Utils {
    public class ImageCompressor {

        public static byte[] BinaryCompress(byte[] base64Image, double percent) {
            try {
                using (var bitmap = SKBitmap.Decode(base64Image)) {
                    if (bitmap == null)
                        return null;
                    using (var mstream = new SKDynamicMemoryWStream())  // Compress Image
                        if (bitmap.Encode(mstream, SKEncodedImageFormat.Jpeg, (int)(100 * (percent > 1 ? 1 : percent))))
                            return StreamToBase64Bytes(mstream);
                    return null;
                }
            } catch {
                return null;
            }
        }

        private static string StreamToBase64String(SKDynamicMemoryWStream mstream) {
            var str = mstream.DetachAsStream();
            var bts = new byte[str.Length];
            str.Read(bts, str.Length);
            return Convert.ToBase64String(bts);
        }

        private static byte[] StreamToBase64Bytes(SKDynamicMemoryWStream mstream) {
            var str = mstream.DetachAsStream();
            var bts = new byte[str.Length];
            str.Read(bts, str.Length);
            return bts;
        }

    }
}
