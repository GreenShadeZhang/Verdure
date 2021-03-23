using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdure.ApplicationCore.Entities
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; protected set; }
    }
}
