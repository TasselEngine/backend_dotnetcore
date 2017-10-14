﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Contract {

    public interface IStatusService : IBusinessService<Status, Error> {

        ICommentServiceProvider Comments { get; }

        ILikesServiceProvider Likes { get; }

        ValueTask<(Status entry, JsonStatus status, Error error)> GetStatusDetailsAsync(string id);

        ValueTask<(Status entry, JsonStatus status, Error error)> GetStatusAbstractAsync(string id);

    }

}