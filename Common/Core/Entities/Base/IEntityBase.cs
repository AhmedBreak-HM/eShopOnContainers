using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Entities.Base
{
    public interface IEntityBase<TKey, TName>
    {
        TKey Id { get; }
        TName Name { get; }
    }
}
