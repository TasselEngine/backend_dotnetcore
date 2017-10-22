using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Helpers;
using Tassel.API.VM.Status;
using Tassel.Model.Models.BsonModels;

namespace Tassel.API.VM {
    public static class ModelCreator {

        public static Model.Models.BsonModels.Status CreateStatus(CreateStatusVM vm) {
            var entry = new Model.Models.BsonModels.Status {
                Content = vm.Content,
                State = EntryState.Published,
                Creator = new BaseCreator { UUID = vm.UserID, UserName = vm.UserName },
                Images = vm.Images.Select(i => new BaseImage {
                    OriginUrl = i.OriginURL ,
                    MiddleUrl = i.MiddleURL,
                    LargeUrl = i.LargeURL,
                    Thumbnail = i.ThumbnailURL,
                    IsFile = false,
                }).ToList()
            };
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
