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

        public DropletModel Droplet { get; private set; }
        public List<ElectrodeModel> Electrodes { get; private set; }
        public bool IsRunning { get; private set; } = false;

        public SimulatorManager()
        {
            Droplet = new();
            Electrodes = new();
        }

        public async void MainLoop()
        {
            IsRunning = true;
            while(IsRunning)
            {
                MoveObjects();
                FinishAnimation();
                ManageElectrodes();

                MainLoopCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(20);
            }
        }

        private void ManageElectrodes()
        {
            if (!Electrodes.Any())
            {
                for (int i = 0; i < 20; i++)
                {
                    Electrodes.Add(new ElectrodeModel());
                }
            }
        }

        private void MoveObjects()
        {
            Droplet.MoveDown(_speed);
        }

        private void FinishAnimation()
        {
            if (Droplet.DistanceFromBottom <= 0)
                EndSimulator();
        }

        public void StartSimulator()
        {
            if (!IsRunning)
            {
                Droplet = new();
                MainLoop();
            }
        }

        public void EndSimulator()
        {
            IsRunning = false;
        }
    }
}
