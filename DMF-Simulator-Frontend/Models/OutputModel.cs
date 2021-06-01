using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public record OutputModel : BaseElementModel
    {
        public int OutputID { get; init; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();
            info.TryAdd("Output ID", OutputID.ToString());
            return info;
        }
    }
}
