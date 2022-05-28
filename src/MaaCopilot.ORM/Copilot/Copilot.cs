using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.ORM.Copilot
{
    public class Copilot : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CoPilotID { get; set; }
        public string GUID { get; set; }
        public string Json { get; set; }
    }
}
