using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Helpers;
using Tassel.API.VM.Status;
using Tassel.Model.Models.BsonModels;

namespace Tassel.API.VM {
    public static class ModelCreator {

        public static Model.Models.BsonModels.Status CreateStatus(Status.CreateStatusVM vm) {
            var entry = new Model.Models.BsonModels.Status {
                Content = vm.Content,
                State = EntryState.Published,
                Creator = new BaseCreator { UUID = vm.UserID, UserName = vm.UserName },
                Images = vm.Images.Select(i => new BaseImage { Base64 = i.Base64, FileSize = i.Size.GetValueOrDefault(), IsFile = true }).ToList()
            };
            entry.Images.ToList().ForEach(i => { // Compression
                if (!i.IsFile) { return; }
                var (base64, thumb) = ImageRender.ImageMutiCompress(i.Base64, 0.2, i.FileSize > 40000 ? 0.1 : 0.2);
                i.Base64 = base64 ?? i.Base64;
                i.Thumbnail = thumb;
            });
            return entry;
        }

        public static Comment CreateComment(CommentInsertVM vm, string parent_id, ModelType parent_type) {
            return new Comment {
                Creator = new BaseCreator { UserName = vm.UName, UUID = vm.UID },
                CommentContent = vm.Content,
                ParentID = parent_id,
                ParentType = parent_type
            };
        }

        public static LikesEntry CreateLike(LikeVM vm, string parent_id, ModelType parent_type) {
            return new LikesEntry {
                User = new BaseCreator { UUID = vm.UserID, UserName = vm.UserName },
                TargetType = parent_type,
                ParentID = parent_id
            };
        }

    }
}
