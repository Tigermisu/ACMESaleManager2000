using ACMESaleManager2000.DataRepositories;
using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public class ItemService : Service<Item>, IItemService
    {
        public ItemService(IRepository<Item> repository) : base(repository)
        {
        }
    }
}
