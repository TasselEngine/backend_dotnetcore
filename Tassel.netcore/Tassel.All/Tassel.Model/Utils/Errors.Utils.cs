using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Utils {

    public struct Error {

        public readonly string Message;
        public readonly string Details;
        public readonly bool IsInvalid;

        public static Error Empty { get; } = new Error();

        public static Error Create(string error, string details = null) {
            return new Error(error, details);
        }

        public Error(string error, string details = null) {
            this.Message = error;
            this.Details = details;
            this.IsInvalid = true;
        }

        public string Read() {
            if (string.IsNullOrEmpty(Message) || !IsInvalid)
                return null;
            return $"{this.Message} : {this.Details ?? "no details"}";
        }

    }

    public static class Errors {
        public const string UnknownError = "unknown error";
        public const string AccessDenied = "access denied";
        public const string CompressFileFailed = "compress file failed";
        public const string CreateFileFailed = "create file failed";
        public const string DeleteNotAllowed = "deleting user is unavaliable.";
        public const string UserNotFound = "user not found.";
        public const string UserExist = "user account is exist already.";
        public const string SaveUserInfosFailed = "save user informations failed.";
        public const string WeiboUserNotFound = "weibo user info is not found.";
        public const string PasswordNotCorrect = "password is not correct.";
        public const string UpdateUserFailed = "update user infos failed.";
        public const string InsertOneFailed = "insert entry failed";
        public const string GetEntryCollFailed = "get bson collection failed";
        public const string QueryEntryFailed = "find entry(s) failed";
        public const string EntryNotExist = "the entry(s) not exist";
        public const string UpdateEntryFailed = "update entry(s) failed";
        public const string DeleteEntryFailed = "delete entry(s) failed";
        public const string EntryFailedIsDeleted = "the entry is not exist.";
        public const string FindUpdateFailed = "no entry to be updated is found.";
    }
}
