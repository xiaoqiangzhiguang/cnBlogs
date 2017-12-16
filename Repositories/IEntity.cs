using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
