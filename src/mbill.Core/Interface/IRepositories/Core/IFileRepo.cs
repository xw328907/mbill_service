﻿using mbill.Core.Domains.Entities.Core;
using mbill.Core.Interface.IRepositories.Base;

namespace mbill.Core.Interface.IRepositories.Core
{
    public interface IFileRepo : IAuditBaseRepo<FileEntity>
    {
        string GetFileUrl(string path);
    }
}