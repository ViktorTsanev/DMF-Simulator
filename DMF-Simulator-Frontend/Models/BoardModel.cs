using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public class BoardModel
    {
        public List<InformationModel> Information { get; set; }
        public List<ElectrodeModel> Electrodes { get; set; }
        public List<ActuatorModel> Actuators { get; set; }
        public List<SensorModel> Sensors { get; set; }
        public List<InputModel> Inputs { get; set; }
        public List<OutputModel> Outputs { get; set; }
        public List<DropletModel> Droplets { get; set; }
        public List<BubbleModel> Bubbles { get; set; }
        public List<UnclassifiedModel> Unclassified { get; set; }
    }
}