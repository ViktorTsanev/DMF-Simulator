using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        public BoardModel BoardModel { get; private set; }
        public BoardModel InitialState { get; init; }
        public List<BoardModel> BoardStates { get; private set; }
        public List<int> AnimationTimePoints { get; private set; }
        public event EventHandler SimulatorStateChanged;
        public static bool IsStarted { get; private set; }
        public static bool IsPaused { get; private set; } = true;
        public static int AnimationSpeedFactor { get; set; } = 1;
        private static int _startSimFromState;

        public SimulatorManager(List<int> animationTimePoints, BoardModel boardModel, List<BoardModel> boardModelNew)
        {
            BoardModel = boardModel;
            AnimationTimePoints = animationTimePoints;

            InitialState = new();
            InitialState.Droplets = new();
            BoardModel.Droplets.ForEach(element => InitialState.Droplets.Add(element with { }));
            InitialState.Electrodes = new();
            BoardModel.Electrodes.ForEach(element => InitialState.Electrodes.Add(element with { }));

            BoardStates = boardModelNew;
        }

        private static void ElementChanges<T>(T oldElement, T newElement) where T : ElementModel
        {
            oldElement.TranslateX += newElement.PositionX - oldElement.PositionX - oldElement.TranslateX;
            //Console.WriteLine("TrX {0} ID {1}", oldElement.TranslateX, oldElement.ID);
            oldElement.TranslateY += newElement.PositionY - oldElement.PositionY - oldElement.TranslateY;
            //Console.WriteLine("TrY {0} ID {1}", oldElement.TranslateY, oldElement.ID);
            oldElement.ScaleX = (double)newElement.SizeX / oldElement.SizeX;
            //Console.WriteLine("ScaleX {0} ID {1}", oldElement.ScaleX, oldElement.ID);
            oldElement.ScaleY = (double)newElement.SizeY / oldElement.SizeY;
            //Console.WriteLine("ScaleY {0} ID {1}", oldElement.ScaleY, oldElement.ID);
        }

        private async Task ProcessChangesAsync()
        {
            foreach (BoardModel newBoard in BoardStates.GetRange(_startSimFromState, BoardStates.Count - _startSimFromState))
            {
                if (IsStarted && !IsPaused)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    ProcessChange(newBoard);
                    sw.Stop();
                    long changeTime = sw.ElapsedMilliseconds;
                    Console.WriteLine("Elapsed after processing changes: {0}", changeTime);
                    
                    sw.Restart();
                    int speed = AnimationSpeedFactor * (AnimationTimePoints.ElementAt(_startSimFromState + 1) - AnimationTimePoints.ElementAt(_startSimFromState));
                    ElementModel.AnimationSpeed = speed;
                    SimulatorStateChanged?.Invoke(this, EventArgs.Empty);
                    long eventTime = sw.ElapsedMilliseconds;
                    Console.WriteLine("Elapsed after invoking event: {0}", eventTime);
                    
                    sw.Restart();
                    speed = speed - changeTime - eventTime <= 0 ? 1 : (int)(speed - changeTime - eventTime);
                    await Task.Delay(speed);
                    sw.Stop();
                    Console.WriteLine("Elapsed after delay: {0}", sw.ElapsedMilliseconds);
                    _startSimFromState++;
                }
                else
                {
                    break;
                }
            }
        }

        private void ProcessChange(BoardModel newBoard)
        {
            if (newBoard.Electrodes != null)
            {
                newBoard.Electrodes.ForEach(e =>
                {
                    ElectrodeModel changedElectrode = BoardModel.Electrodes.Where(t => t.ID == e.ID).FirstOrDefault();
                    if (changedElectrode != null)
                    {
                        ElementChanges(changedElectrode, e);
                        changedElectrode.Status = e.Status;
                    }
                });
            }

            if (newBoard.Droplets != null)
            {
                newBoard.Droplets.ForEach(d =>
                {
                    DropletModel changedDroplet = BoardModel.Droplets.Where(t => t.ID == d.ID).FirstOrDefault();
                    if (changedDroplet != null)
                    {
                        ElementChanges(changedDroplet, d);
                        changedDroplet.Substance_Name = d.Substance_Name;
                        changedDroplet.Temperature = d.Temperature;
                    }
                });

                // Add newly created droplets to the current list (board)
                IEnumerable<DropletModel> newDroplets = newBoard.Droplets.Where(p => !BoardModel.Droplets.Any(p2 => p2.ID == p.ID));
                BoardModel.Droplets.AddRange(newDroplets);
            }
        }

        public async Task StartSimulatorAsync()
        {
            if (!IsStarted || IsPaused)
            {
                IsStarted = true;
                IsPaused = false;
                if (_startSimFromState < BoardStates.Count)
                {
                    await ProcessChangesAsync();
                }
                if (_startSimFromState >= BoardStates.Count)
                {
                    await StopSimulatorAsync();
                }
            }
        }

        public static void PauseSimulator()
        {
            IsPaused = true;
        }

        public async Task StopSimulatorAsync()
        {
            IsStarted = false;
            IsPaused = true;
            _startSimFromState = 0;

            BoardModel.Droplets = new();
            InitialState.Droplets.ForEach(element => BoardModel.Droplets.Add(element with { }));
            BoardModel.Electrodes = new();
            InitialState.Electrodes.ForEach(element => BoardModel.Electrodes.Add(element with { }));

            SimulatorStateChanged?.Invoke(this, EventArgs.Empty);
            await Task.Delay(ElementModel.AnimationSpeed);
        }
    }
}
