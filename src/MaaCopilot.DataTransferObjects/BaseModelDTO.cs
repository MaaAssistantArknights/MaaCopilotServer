using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.DataTransferObjects
{
    public abstract class BaseModelDTO
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool Enabled { get; set; }
    }
}
