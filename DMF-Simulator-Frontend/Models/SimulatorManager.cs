using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DMF_Simulator_Frontend.Components;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        public BoardModel BoardModel { get; private set; }
        public BoardModel InitialState { get; init; }
        public List<BoardModel> BoardStates { get; private set; }
        public List<int> AnimationTimePoints { get; private set; }
        public event EventHandler<AnimationEventArgs> SimulatorStateChanged;
        public static bool IsStarted { get; private set; }
        public static bool IsPaused { get; private set; } = true;
        public static int AnimationSpeedFactor { get; set; } = 1;
        private static int _startSimFromState;
        private bool _processingChanges;

        public SimulatorManager(SimulatorData simulatorData)
        {
            BoardModel = simulatorData.InitialBoard;
            AnimationTimePoints = simulatorData.AnimationTimePoints;

            InitialState = new();
            InitialState.CopySampleBoard(BoardModel);

            BoardStates = simulatorData.BoardStates;
        }

        private async Task ProcessChangesAsync()
        {
            foreach (BoardModel newBoard in BoardStates.GetRange(_startSimFromState, BoardStates.Count - _startSimFromState))
            {
                _processingChanges = true;
                if (IsStarted && !IsPaused)
                {
                    // Process 1 change.
                    Stopwatch sw = Stopwatch.StartNew();
                    ProcessChange(newBoard);
                    sw.Stop();
                    long changeTime = sw.ElapsedMilliseconds;
                    Console.WriteLine("Elapsed after processing changes: {0}", changeTime);

                    // Raise event. Rerender simulator component.
                    sw.Restart();
                    int speed = AnimationSpeedFactor * (AnimationTimePoints.ElementAt(_startSimFromState + 1) - AnimationTimePoints.ElementAt(_startSimFromState));
                    ElementModel.AnimationSpeed = speed;

                    AnimationEventArgs args = new();
                    args.CurrentAnimationName = AnimationTimePoints.ElementAt(_startSimFromState + 1).ToString() + ".json";
                    SimulatorStateChanged?.Invoke(this, args);

                    long eventTime = sw.ElapsedMilliseconds;
                    Console.WriteLine("Elapsed after invoking event: {0}", eventTime);

                    // Delay processing to follow animation timing.
                    sw.Restart();
                    speed = speed - changeTime - eventTime <= 0 ? 1 : (int)(speed - changeTime - eventTime);
                    await Task.Delay(speed);
                    sw.Stop();
                    Console.WriteLine("Elapsed after delay: {0}", sw.ElapsedMilliseconds);
                    _startSimFromState++;
                }
                else
                {
                    _processingChanges = false;
                    break;
                }
            }
        }

        private void ProcessChange(BoardModel newBoard)
        {
            if (newBoard.Electrodes != null)
            {
                if (BoardModel.Electrodes == null)
                {
                    BoardModel.Electrodes = new();
                }
                newBoard.Electrodes.ForEach(e =>
                {
                    ElectrodeModel changedElectrode = BoardModel.Electrodes.Where(t => t.ID == e.ID).FirstOrDefault();
                    if (changedElectrode != null)
                    {
                        changedElectrode.ApplyElementChanges(e);
                    }
                });
            }

            if (newBoard.Actuators != null)
            {
                if (BoardModel.Actuators == null)
                {
                    BoardModel.Actuators = new();
                }
                newBoard.Actuators.ForEach(a =>
                {
                    ActuatorModel changedActuator = BoardModel.Actuators.Where(t => t.ID == a.ID).FirstOrDefault();
                    if (changedActuator != null)
                    {
                        changedActuator.ApplyElementChanges(a);
                    }
                });
            }

            if (newBoard.Sensors != null)
            {
                if (BoardModel.Sensors == null)
                {
                    BoardModel.Sensors = new();
                }
                newBoard.Sensors.ForEach(s =>
                {
                    SensorModel changedSensor = BoardModel.Sensors.Where(t => t.ID == s.ID).FirstOrDefault();
                    if (changedSensor != null)
                    {
                        changedSensor.ApplyElementChanges(s);
                    }
                });
            }

            if (newBoard.Droplets != null)
            {
                if (BoardModel.Droplets == null)
                {
                    BoardModel.Droplets = new();
                }
                newBoard.Droplets.ForEach(d =>
                {
                    DropletModel changedDroplet = BoardModel.Droplets.Where(t => t.ID == d.ID).FirstOrDefault();
                    if (changedDroplet != null)
                    {
                        changedDroplet.ApplyElementChanges(d);
                    }
                });

                // Add newly created droplets to the current board.
                BoardModel.Droplets.AddRange(GetNewElements(BoardModel.Droplets, newBoard.Droplets));
            }

            if (newBoard.Bubbles != null)
            {
                if (BoardModel.Bubbles == null)
                {
                    BoardModel.Bubbles = new();
                }
                newBoard.Bubbles.ForEach(b =>
                {
                    BubbleModel changedBubble = BoardModel.Bubbles.Where(t => t.ID == b.ID).FirstOrDefault();
                    if (changedBubble != null)
                    {
                        changedBubble.ApplyElementChanges(b);
                    }
                });

                // Add newly created bubbles to the current board.
                BoardModel.Bubbles.AddRange(GetNewElements(BoardModel.Bubbles, newBoard.Bubbles));
            }
        }

        private static IEnumerable<T> GetNewElements<T>(IList<T> oldElementList, IList<T> newElementList) where T : ElementModel
        {
            return newElementList.Where(p => !oldElementList.Any(p2 => p2.ID == p.ID)).Select(element => element with { Visible = SimulatorContainer.IsVisible(oldElementList) });
        }

        public async Task StartSimulatorAsync()
        {
            // Only start simulator if no changes are being processed and it's not started yet or it's paused.
            if ((!IsStarted || IsPaused) && !_processingChanges)
            {
                Console.WriteLine("Starting from: {0}", _startSimFromState);
                IsStarted = true;
                IsPaused = false;
                if (_startSimFromState < BoardStates.Count)
                {
                    await ProcessChangesAsync();
                    _processingChanges = false;
                }
                if (_startSimFromState >= BoardStates.Count)
                {
                    await StopSimulatorAsync();
                }
            }
        }

        public async Task PauseSimulatorAsync()
        {
            // Pause simulator.
            IsPaused = true;

            // Wait for async Processing changes to finish.
            while (_processingChanges)
            {
                await Task.Delay(1);
            }
        }

        public async Task StopSimulatorAsync()
        {
            // Stop simulator.
            IsStarted = false;
            IsPaused = true;

            // Wait for async Processing changes to finish.
            while (_processingChanges)
            {
                await Task.Delay(1);
            }

            // Reset to initial state.
            _startSimFromState = 0;
            BoardModel.ClearBoard();
            BoardModel.CopySampleBoard(InitialState);

            // Raise event. Rerender simulator component.
            AnimationEventArgs args = new();
            args.CurrentAnimationName = AnimationTimePoints.FirstOrDefault().ToString() + ".json";
            SimulatorStateChanged?.Invoke(this, args);
            await Task.Delay(ElementModel.AnimationSpeed);
        }
    }
}
