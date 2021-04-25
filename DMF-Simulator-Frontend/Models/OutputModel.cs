using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public class OutputModel : BaseElementModel
    {
        public int OutputID { get; set; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();
            info.TryAdd("Output ID", OutputID.ToString());
            return info;
        }
    }
}
