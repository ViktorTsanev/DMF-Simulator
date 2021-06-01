using System.Collections.Generic;
using System.Linq;

namespace DMF_Simulator_Frontend.Models
{
    public record ActuatorModel : ElementModel
    {
        public int ActuatorID { get; init; }
        public string Type { get; init; }
        public int ActualTemperature { get; set; }
        public int DesiredTemperature { get; set; }
        public bool Status { get; set; }
        public int NextDesiredTemperature { get; set; }
        public bool NextStatus { get; set; }
        public List<List<int>> Corners { get; init; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();
            info.TryAdd("Actuator ID", ActuatorID.ToString());
            info.TryAdd("Type", Type);
            info.TryAdd("Actual temp", ActualTemperature.ToString());
            info.TryAdd("Desired temp", DesiredTemperature.ToString());
            info.TryAdd("Status", (Status ? "On" : "Off"));
            info.TryAdd("Next desired temp", NextDesiredTemperature.ToString());
            info.TryAdd("Next status", NextStatus.ToString());
            if (Corners != null)
            {
                string coords = "";
                foreach (var corner in Corners)
                {
                    int x = corner.First() + PositionX;
                    int y = corner.Last() + PositionY;
                    coords += "(" + x + ", " + y + ") ";
                }
                info.TryAdd("Corners", coords);
            }
            return info;
        }

        public override void ApplyElementChanges(BaseElementModel newElement)
        {
            base.ApplyElementChanges(newElement);
            ActualTemperature = ((ActuatorModel)newElement).ActualTemperature;
            DesiredTemperature = ((ActuatorModel)newElement).DesiredTemperature;
            Status = ((ActuatorModel)newElement).Status;
            NextDesiredTemperature = ((ActuatorModel)newElement).NextDesiredTemperature;
            NextStatus = ((ActuatorModel)newElement).NextStatus;
        }
    }
}
