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

        private void IndividualChanges<T>(T oldElement, T newElement) where T : ElementModel
        {
            oldElement.TranslateX += newElement.PositionX - oldElement.PositionX - oldElement.TranslateX;
            Console.WriteLine("TrX {0} ID {1}", oldElement.TranslateX, oldElement.ID);
            oldElement.TranslateY += newElement.PositionY - oldElement.PositionY - oldElement.TranslateY;
            Console.WriteLine("TrY {0} ID {1}", oldElement.TranslateY, oldElement.ID);
            oldElement.ScaleX = (double)newElement.SizeX / oldElement.SizeX;
            Console.WriteLine("ScaleX {0} ID {1}", oldElement.ScaleX, oldElement.ID);
            oldElement.ScaleY = (double)newElement.SizeY / oldElement.SizeY;
            Console.WriteLine("ScaleY {0} ID {1}", oldElement.ScaleY, oldElement.ID);
        }

        private async Task ProcessChangesAsync()
        {
            foreach (BoardModel b in BoardModelNew)
            {
                BoardModel.Electrodes.ForEach(delegate (ElectrodeModel e)
                {
                    if (b.Electrodes != null)
                    {
                        ElectrodeModel newElectrode = b.Electrodes.Where(t => t.ID == e.ID).FirstOrDefault();
                        if (newElectrode != null)
                        {
                            IndividualChanges(e, newElectrode);
                            e.Status = newElectrode.Status;
                        }
                    }
                });

                BoardModel.Droplets.ForEach(delegate (DropletModel d)
                {
                    if (b.Droplets != null)
                    {
                        DropletModel newDroplet = b.Droplets.Where(t => t.ID == d.ID).FirstOrDefault();
                        if (newDroplet != null)
                        {
                            IndividualChanges(d, newDroplet);
                            d.SubstanceName = newDroplet.SubstanceName;
                            d.Temperature = newDroplet.Temperature;
                        }
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
