using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        private readonly int _speed = 2;
        public event EventHandler MainLoopCompleted;
        public List<DropletModel> Droplets { get; private set; }
        public List<ElectrodeModel> Electrodes { get; private set; }
        public bool IsRunning { get; private set; } = false;

        public SimulatorManager(BoardModel boardModel)
        {
            Droplets = boardModel.Droplets;
            Electrodes = boardModel.Electrodes;
        }

        public async void MainLoop()
        {
            IsRunning = true;
            while(IsRunning)
            {
                if (Droplets != null)
                {
                    MoveObjects();
                    FinishAnimation();
                }

                ManageElectrodes();

                MainLoopCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(20);
            }
        }

        private void ManageElectrodes()
        {
            if (!Electrodes.Any())
            {
            }
        }

        private void MoveObjects()
        {
            Droplets.First().MoveDown(_speed);
        }

        private void FinishAnimation()
        {
            if (Droplets.First().PositionY <= 0)
                EndSimulator();
        }

        public void StartSimulator()
        {
            if (!IsRunning)
            {
                //Droplets = 
                MainLoop();
            }
        }

        public void EndSimulator()
        {
            IsRunning = false;
        }
    }
}
