using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public record InputModel : BaseElementModel
    {
        public int InputID { get; init; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();
            info.TryAdd("input ID", InputID.ToString());
            return info;
        }
    }
}
