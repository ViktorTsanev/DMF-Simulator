using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        public BoardModel BoardModel { get; private set; }
        public event EventHandler MainLoopCompleted;
        public bool IsRunning { get; private set; } = false;
        private readonly int _speed = 2;

        public SimulatorManager(BoardModel boardModel)
        {
            BoardModel = boardModel;
        }

        public async void MainLoop()
        {
            IsRunning = true;
            while(IsRunning)
            {
                if (BoardModel.Droplets != null)
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
            if (!BoardModel.Electrodes.Any())
            {
            }
        }

        private void MoveObjects()
        {
            if (BoardModel.Droplets.First().PositionY < BoardModel.Information.FirstOrDefault().SizeY - BoardModel.Droplets.First().SizeY)
            {
                BoardModel.Droplets.First().MoveDown(_speed);
            }
        }

        private void FinishAnimation()
        {
            if (BoardModel.Droplets.First().PositionY >= BoardModel.Information.FirstOrDefault().SizeY - BoardModel.Droplets.First().SizeY)
            {
                EndSimulator();
            }
        }

        public void StartSimulator()
        {
            if (!IsRunning)
            {
                MainLoop();
            }
        }

        public void EndSimulator()
        {
            IsRunning = false;
        }
    }
}
