﻿using mbill.Core.Domains.Entities.Core;
using mbill.Core.Interface.IRepositories.Base;

namespace mbill.Core.Interface.IRepositories.Core
{
    public interface ILogRepo : IAuditBaseRepo<LogEntity>
    {
    }
}
