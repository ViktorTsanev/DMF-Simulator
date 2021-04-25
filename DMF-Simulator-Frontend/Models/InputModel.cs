using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public class InputModel : BaseElementModel
    {
        public int InputID { get; set; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();
            info.TryAdd("inout ID", InputID.ToString());
            return info;
        }
    }
}
