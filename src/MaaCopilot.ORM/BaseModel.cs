using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.ORM
{
    public abstract class BaseModel
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool Enabled { get; set; }

    }
}
