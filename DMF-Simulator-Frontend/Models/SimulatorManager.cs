using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        public BoardModel BoardModel { get; private set; }
        public List<BoardModel> BoardModelNew { get; private set; }
        public event EventHandler MainLoopCompleted;
        public bool IsRunning { get; private set; }

        public SimulatorManager(BoardModel boardModel, List<BoardModel> boardModelNew)
        {
            BoardModel = boardModel;
            BoardModelNew = boardModelNew;
        }

        public void MainLoop()
        {
            IsRunning = true;

            while (IsRunning)
            {
                EndSimulator();
            }
        }

        private async Task ProcessChangesAsync()
        {
            foreach (BoardModel b in BoardModelNew)
            {
                BoardModel.Electrodes.ForEach(delegate (ElectrodeModel e)
                {
                    ElectrodeModel newElectrode = b.Electrodes.Where(t => t.ID == e.ID).FirstOrDefault();
                    if (newElectrode != null)
                    {
                        e.TranslateX += newElectrode.PositionX - e.PositionX - e.TranslateX;
                        Console.WriteLine("TrX {0} ID {1}", e.TranslateX, e.ID);
                        e.TranslateY += newElectrode.PositionY - e.PositionY - e.TranslateY;
                        Console.WriteLine("TrY {0} ID {1}", e.TranslateY, e.ID);
                        e.ScaleX = (double)newElectrode.SizeX / e.SizeX;
                        Console.WriteLine("ScaleX {0} ID {1}", e.ScaleX, e.ID);
                        e.ScaleY = (double)newElectrode.SizeY / e.SizeY;
                        Console.WriteLine("ScaleY {0} ID {1}", e.ScaleY, e.ID);
                        e.Status = newElectrode.Status;
                    }
                });
                BoardModel.Droplets.ForEach(delegate (DropletModel d)
                {
                    DropletModel newDroplet = b.Droplets.Where(t => t.ID == d.ID).FirstOrDefault();
                    if (newDroplet != null)
                    {
                        d.TranslateX += newDroplet.PositionX - d.PositionX - d.TranslateX;
                        Console.WriteLine("TrX {0} ID {1}", d.TranslateX, d.ID);
                        d.TranslateY += newDroplet.PositionY - d.PositionY - d.TranslateY;
                        Console.WriteLine("TrY {0} ID {1}", d.TranslateY, d.ID);
                        d.ScaleX = (double)newDroplet.SizeX / d.SizeX;
                        Console.WriteLine("ScaleX {0} ID {1}", d.ScaleX, d.ID);
                        d.ScaleY = (double)newDroplet.SizeY / d.SizeY;
                        Console.WriteLine("ScaleY {0} ID {1}", d.ScaleY, d.ID);
                        d.Color = newDroplet.Color;
                    }
                });
                
                MainLoopCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(1000);
            }
        }

        public async Task StartSimulatorAsync()
        {
            await ProcessChangesAsync();
            //MainLoopCompleted?.Invoke(this, EventArgs.Empty);
            /*if (!IsRunning)
            {
                MainLoop();
            }*/
        }

        public void EndSimulator()
        {
            IsRunning = false;
        }
    }
}
