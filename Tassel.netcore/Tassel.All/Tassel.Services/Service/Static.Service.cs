using BWS.Utils.NetCore.Format;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Utils;
using Tassel.Services.Contract;

namespace Tassel.Services.Service {

    public class StaticService : IStaticService {

        private const string origin = "imgs";
        private const string normal = "middle";
        private const string large = "large";
        private const string thumbnail = "tbs";

        private IHostingEnvironment env;
        private IFileProvider provider;

        public StaticService(IHostingEnvironment env) {
            this.env = env;
            this.provider = this.env.WebRootFileProvider;
        }

        public async ValueTask<(bool succeed, Error error, ImageResult result)> CreateIamgeResourceAsync(string base64FileString) {
            var bts = Convert.FromBase64String(base64FileString);
            return await CreateIamgeResourceAsync(bts);
        }

        public async ValueTask<(bool succeed, Error error, ImageResult result)> CreateIamgeResourceAsync(byte[] base64Image) {
            var unq_name = $"{Guid.NewGuid().ToString()}-{DateTime.UtcNow.ToUnix()}";
            var end = new ImageResult ();
            try {
                end.OriginImagePath = await CreateImageByCompressPercentAsync(base64Image, unq_name, origin);
                end.LargeImagePath = await CreateImageByCompressPercentAsync(base64Image, unq_name, large, 0.7);
                end.MiddleImagePath = await CreateImageByCompressPercentAsync(base64Image, unq_name, normal, 0.4);
                end.ThumbnailPath = await CreateImageByCompressPercentAsync(base64Image, unq_name, thumbnail, 0.1);
            } catch (Exception e) {
                return (false, Error.Create(Errors.CreateFileFailed, e.Message), null);
            }
            return (true, Error.Empty, end);
        }

        private async ValueTask<string> CreateImageByCompressPercentAsync(byte[] base64Image, string unq_name, string prefix ,double percent = 1) {
            var name = $"/resources/images/{prefix}/{unq_name}.png";
            using (var logFile = File.Create(this.env.WebRootPath + name))
            using (var logWriter = new BufferedStream(logFile)) {
                var bts = base64Image;
                if (percent < 1)
                    bts = ImageCompressor.BinaryCompress(bts, percent);
                await logWriter.WriteAsync(bts, 0, bts.Length);
                return name;
            };
        }

        public ValueTask<(bool succeed, Error error)> DeleteIamgeResourceAsync(string base64FileString) {
            throw new NotImplementedException();
        }

    }
}
