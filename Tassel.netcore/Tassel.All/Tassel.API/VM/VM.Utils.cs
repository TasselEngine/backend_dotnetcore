using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Helpers;
using Tassel.API.VM.Status;
using Tassel.Model.Models.BsonModels;

namespace Tassel.API.VM {
    public static class ModelCreator {

        public static Model.Models.BsonModels.Status CreateStatus(CreateStatusVM vm, string avatar = null) {
            var entry = new Model.Models.BsonModels.Status {
                Content = vm.Content,
                State = EntryState.Published,
                Creator = new BaseCreator {
                    UUID = vm.UserID,
                    UserName = vm.UserName,
                    AvatarUrl = avatar
                },
                Images = vm.Images.Where(i => i != null).Select(i => new BaseImage {
                    OriginUrl = i.OriginURL,
                    Thumbnail = i.ThumbnailURL,
                    Width = i.Width,
                    Height = i.Height,
                    IsFile = false,
                }).ToList()
            };
            return entry;
        }

        public static Comment CreateComment(CommentAddVM vm, string parent_id, ModelType parent_type, string avatar = null) {
            return new Comment {
                Creator = new BaseCreator {
                    UserName = vm.UName,
                    UUID = vm.UID,
                    AvatarUrl = avatar
                },
                Mentioned = vm.MUID == null || vm.MName == null ? null :new BaseCreator {
                    UUID = vm.MUID,
                    UserName = vm.MName
                },
                CommentContent = vm.Content,
                ParentID = parent_id,
                ParentType = parent_type
            };
        }

        public static LikesEntry CreateLike(LikeVM vm, string parent_id, ModelType parent_type, string avatar = null) {
            return new LikesEntry {
                User = new BaseCreator {
                    UUID = vm.UserID,
                    UserName = vm.UserName,
                    AvatarUrl = avatar
                },
                TargetType = parent_type,
                ParentID = parent_id
            };
        }

    }
}
