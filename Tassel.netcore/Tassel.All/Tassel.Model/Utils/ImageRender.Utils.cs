using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Utils {
    public class ImageCompressor {

        public static (byte[]result, string error) BinaryCompress(byte[] base64Image, double percent, int? minWidth = null) {
            try {
                using (var bitmap = SKBitmap.Decode(base64Image)) {
                    if (bitmap == null)
                        return (null, "base64Image is null.");
                    if (minWidth != null && bitmap.Width < minWidth)
                        percent = 1;
                    using (var mstream = new SKDynamicMemoryWStream())  // Compress Image
                        if (bitmap.Encode(mstream, SKEncodedImageFormat.Jpeg, (int)(100 * (percent > 1 ? 1 : percent))))
                            return (StreamToBase64Bytes(mstream), null);
                    return (null, "encode picture failed : Encode action failed.");
                }
            } catch (Exception e) {
                return (null, "draw picture failed : " + e.Message);
            }
        }

        public static (byte[] result, string error) BinaryCutCompress(byte[] base64Image, double percent, int? minWidth = null, int?maxWidth = null) {
            try {
                using (var bitmap = SKBitmap.Decode(base64Image)) {
                    if (bitmap == null)
                        return (null, "cut - base64Image is null.");

                    if(minWidth!=null && bitmap.Width< minWidth) 
                        percent = 1;

                    var isWide = bitmap.Width > bitmap.Height;
                    var num = isWide ? bitmap.Height : bitmap.Width;
                    
                    var p_num = 1.0;
                    if (maxWidth != null && bitmap.Width > maxWidth)
                        p_num = 1 * (maxWidth.GetValueOrDefault() / (double)bitmap.Width);

                    var pad = ((isWide ? bitmap.Width : bitmap.Height) - num) / 2;
                    var ex_num = (int)(p_num * num);
                    using (var newBitmap = new SKBitmap(ex_num, ex_num)) {
                        try {
                            using (var canvas = new SKCanvas(newBitmap)) {
                                canvas.Scale((float)p_num);
                                canvas.DrawBitmap(bitmap, isWide ? -pad : 0, isWide ? 0 : -pad);
                            }
                            using (var mstream = new SKDynamicMemoryWStream())
                                if (newBitmap.Encode(mstream, SKEncodedImageFormat.Jpeg, (int)(100 * (percent > 1 ? 1 : percent))))
                                    return (StreamToBase64Bytes(mstream),null);
                            return (null, "cut - encode picture failed : Encode action failed.");
                        } catch (Exception e){
                            return (null, "cut - draw canvas failed : " + e.Message);
                        }
                    }
                }
            } catch (Exception e) {
                return (null, "cut - draw picture failed : " + e.Message);
            }
        }

        public static (int width, int height) BinarySize(byte[] base64Image) {
            try {
                using (var bitmap = SKBitmap.Decode(base64Image)) {
                    if (bitmap == null)
                        return (0, 0);
                    return (bitmap.Width, bitmap.Height);
                }
            } catch {
                return (0, 0);
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
