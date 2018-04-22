﻿using System.Collections.Generic;

namespace ACMESaleManager2000.DataRepositories
{
    public interface IRepository<TDomainObject>
    {
        List<TDomainObject> GetAll();
    }
}