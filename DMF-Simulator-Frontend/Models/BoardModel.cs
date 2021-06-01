using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DMF_Simulator_Frontend.Models
{
    public record BoardModel
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

        public BoardModel()
        {
            Information = new();
            Electrodes = new();
            Actuators = new();
            Sensors = new();
            Inputs = new();
            Outputs = new();
            Droplets = new();
            Bubbles = new();
            Unclassified = new();
        }

        public void ClearBoard()
        {
            foreach (PropertyInfo prop in typeof(BoardModel).GetProperties())
            {
                if (prop.GetValue(this) is IList baseElementList)
                {
                    baseElementList.Clear();
                }
            }
        }

        public void CopySampleBoard(BoardModel sampleBoard)
        {
            if (sampleBoard.Information != null)
            {
                if (Information == null)
                {
                    Information = new();
                }
                sampleBoard.Information.ForEach(element => Information.Add(element with { }));
            }
            if (sampleBoard.Electrodes != null)
            {
                if (Electrodes == null)
                {
                    Electrodes = new();
                }
                sampleBoard.Electrodes.ForEach(element => Electrodes.Add(element with { }));
            }
            if (sampleBoard.Actuators != null)
            {
                if (Actuators == null)
                {
                    Actuators = new();
                }
                sampleBoard.Actuators.ForEach(element => Actuators.Add(element with { }));
            }
            if (sampleBoard.Sensors != null)
            {
                if (Sensors == null)
                {
                    Sensors = new();
                }
                sampleBoard.Sensors.ForEach(element => Sensors.Add(element with { }));
            }
            if (sampleBoard.Inputs != null)
            {
                if (Inputs == null)
                {
                    Inputs = new();
                }
                sampleBoard.Inputs.ForEach(element => Inputs.Add(element with { }));
            }
            if (sampleBoard.Outputs != null)
            {
                if (Outputs == null)
                {
                    Outputs = new();
                }
                sampleBoard.Outputs.ForEach(element => Outputs.Add(element with { }));
            }
            if (sampleBoard.Droplets != null)
            {
                if (Droplets == null)
                {
                    Droplets = new();
                }
                sampleBoard.Droplets.ForEach(element => Droplets.Add(element with { }));
            }
            if (sampleBoard.Bubbles != null)
            {
                if (Bubbles == null)
                {
                    Bubbles = new();
                }
                sampleBoard.Bubbles.ForEach(element => Bubbles.Add(element with { }));
            }
            if (sampleBoard.Unclassified != null)
            {
                if (Unclassified == null)
                {
                    Unclassified = new();
                }
                sampleBoard.Unclassified.ForEach(element => Unclassified.Add(element with { }));
            }
        }
    }
}
