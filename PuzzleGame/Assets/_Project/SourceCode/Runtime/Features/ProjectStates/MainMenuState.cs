using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Features.Game;
using Runtime.Features.Game.Ui.MainMenu;
using Runtime.Features.Game.Ui.StartPuzzlePanel;
using Runtime.Infrastructure;

namespace Runtime.Features.ProjectStates
{
    public class MainMenuState : BaseStateWithoutData
    {
        private readonly MainMenuScreen _mainMenuScreen;
        private readonly StateMachine _stateMachine;
        private readonly List<LevelConfig> _levelConfigs;

        public MainMenuState(StateMachine stateMachine, MainMenuScreen mainMenuScreen, List<LevelConfig> levelConfigs)
        {
            _levelConfigs = levelConfigs;
            _stateMachine = stateMachine;
            _mainMenuScreen = mainMenuScreen;
        }

        public override void OnEnter()
        {
            _mainMenuScreen.ShowAsync(
                new MainMenuScreen.Info {LevelConfigs = _levelConfigs}).Forget();

            _mainMenuScreen.EventPuzzleSelected += OnEventPuzzleSelected;
        }

        private void OnEventPuzzleSelected(int size, StartPuzzleType startPuzzleType)
        {
            switch (startPuzzleType)
            {
                case StartPuzzleType.Free:
                    _stateMachine.SetCurrentStateWithData<GameState, GameState.GameStateData>(
                        new GameState.GameStateData {Size = size});

                    break;
                case StartPuzzleType.Paid:
                    //
                    //
                    // Data.Money -= price;
                    //
                    _stateMachine.SetCurrentStateWithData<GameState, GameState.GameStateData>(
                        new GameState.GameStateData {Size = size});

                    break;
                case StartPuzzleType.ForAd:
                    //
                    //  Advertisement.ShowInterstitial(OnComplete: ()=>{
                    //_stateMachine.SetCurrentStateWithData<GameState, GameState.GameStateData>(
                    //    new GameState.GameStateData {Size = size});
                    // })
                    //

                    break;
                default: throw new ArgumentOutOfRangeException(nameof(startPuzzleType), startPuzzleType, null);
            }
        }

        public override void OnExit()
        {
            _mainMenuScreen.HideAsync();
            _mainMenuScreen.EventPuzzleSelected -= OnEventPuzzleSelected;
        }
    }
}