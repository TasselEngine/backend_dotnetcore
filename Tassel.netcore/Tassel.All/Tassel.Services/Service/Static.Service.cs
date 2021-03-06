﻿using BWS.Utils.NetCore.Format;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Utils;
using Tassel.Services.Contract;
using Tassel.Services.Utils.Helpers;

namespace Tassel.Services.Service {

    public class StaticService : IStaticService {

        private const string origin = "images/imgs";
        private const string thumbnail = "images/tbs";

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
            var end = new ImageResult();
            try {
                var error = default(string);
                (end.OriginImagePath, error) = await CreateImageByCompressPercentAsync(base64Image, unq_name, origin, 0.5);
                if (string.IsNullOrEmpty(end.OriginImagePath))
                    return (false, Error.Create(Errors.CompressFileFailed, error), null);
                (end.ThumbnailPath, error) = await CreateImageByCompressPercentAsync(base64Image, unq_name, thumbnail, 0.1, true);
                if (string.IsNullOrEmpty(end.ThumbnailPath))
                    return (false, Error.Create(Errors.CompressFileFailed, error), null);
                (end.Width, end.Height) = ImageCompressor.BinarySize(base64Image);
            } catch (Exception e) {
                return (false, Error.Create(Errors.CreateFileFailed, e.Message), null);
            }
            return (true, Error.Empty, end);
        }

        private async ValueTask<(string result, string error)> CreateImageByCompressPercentAsync(byte[] base64Image, string unq_name, string prefix, double percent = 1, bool cut = false) {
            var name = $"/resources/{prefix}/{unq_name}.png";
            var bts = base64Image;
            var error = default(string);
            if (percent < 1) {
                (bts, error) = cut ? ImageCompressor.BinaryCutCompress(bts, percent, 320, 1600) :
                    ImageCompressor.BinaryCompress(bts, percent, 800);
            }
            if (!string.IsNullOrEmpty(error))
                return (null, error);
            using (var logFile = File.Create(this.env.WebRootPath + name))
            using (var logWriter = new BufferedStream(logFile)) {
                await logWriter.WriteAsync(bts, 0, bts.Length);
                return (name, null);
            };
        }

        public ValueTask<(bool succeed, Error error)> DeleteIamgeResourceAsync(string base64FileString) {
            throw new NotImplementedException();
        }

        public (bool succeed, Error error, IList<KeyValuePair<string, string>> images) GetTiebaImagesGroup() {
            return (true, Error.Empty, GlobalStickersHelper.TiebaModdleGroup);
        }

        public (bool succeed, Error error, IList<KeyValuePair<string, string>> images) GetSinaOthersStickersGroup() {
            return (true, Error.Empty, GlobalStickersHelper.SinaOthersGroup);
        }

        public (bool succeed, Error error, IList<KeyValuePair<string, string>> images) GetSinaPopStickersGroup() {
            return (true, Error.Empty, GlobalStickersHelper.SinaPopGroup);
        }

        public (bool succeed, Error error, IList<KeyValuePair<string, string>> images) GetSinaRoleStickersGroup() {
            return (true, Error.Empty, GlobalStickersHelper.SinaRolesGroup);
        }
    }
}
