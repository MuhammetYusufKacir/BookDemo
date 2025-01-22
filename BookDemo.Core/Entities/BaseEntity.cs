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
        public int userCratedId { get; set; }
        public int userUpdatedId { get; set; }

    }
}
