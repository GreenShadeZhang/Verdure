using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verdure.Core;

namespace Verdure.Infrastructure
{
    public class IdGenerator : IIdGenerator
    {
        public string Generate() => Guid.NewGuid().ToString("N");
    }
}
