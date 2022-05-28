

namespace MaaCopilot.DataTransferObjects.Copilot
{
    public class CopilotDTO : BaseModelDTO
    {
        public int CoPilotID { get; set; }
        public string GUID { get; set; }
        public string Json { get; set; }
    }
    public class UpdateCopilotDTO : CopilotDTO
    {

    }
}
