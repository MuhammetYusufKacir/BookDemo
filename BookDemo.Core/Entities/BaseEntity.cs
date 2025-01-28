using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDemo.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; } 
        public int UserCratedId { get; set; }
        public int UserUpdatedId { get; set; }

    }
}
