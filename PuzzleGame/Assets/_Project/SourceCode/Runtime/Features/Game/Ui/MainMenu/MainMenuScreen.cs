using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Features.Game.Ui.MainMenu
{
    public class MainMenuScreen : BaseScreen<MainMenuScreen.Info>
    {
        [SerializeField]
        private StartPuzzlePanel.StartPuzzlePanel _startPuzzlePanel;

        [SerializeField]
        private PuzzleLevelButton _puzzleLevelButtonPrefab;

        [SerializeField]
        private Transform _levelsRoot;

        private List<PuzzleLevelButton> _puzzleLevelButtons = new();

        public event Action<int> EventPuzzleSelected;

        public async override UniTask ShowAsync(Info info = null, CancellationToken cancellationToken = default)
        {
            gameObject.SetActive(true);
            _startPuzzlePanel.EventStartPuzzleButtonClicked += OnEventStartPuzzleButtonClicked;
            for (var i = 0; i < info.LevelConfigs.Count; i++)
            {
                var puzzleLevelButton = Instantiate(_puzzleLevelButtonPrefab, _levelsRoot);
                puzzleLevelButton.Inititalize(info.LevelConfigs[i]);
                puzzleLevelButton.EventButtonClicked += OnPuzzleLevelClicked;
                _puzzleLevelButtons.Add(puzzleLevelButton);
            }
        }

        public async override UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _startPuzzlePanel.EventStartPuzzleButtonClicked += OnEventStartPuzzleButtonClicked;
            foreach (var puzzleLevelButton in _puzzleLevelButtons)
            {
                puzzleLevelButton.Deinitialize();
                puzzleLevelButton.EventButtonClicked -= OnPuzzleLevelClicked;
                Destroy(puzzleLevelButton.gameObject);
            }

            _puzzleLevelButtons.Clear();

            _startPuzzlePanel.HideAsync();

            gameObject.SetActive(false);
        }

        private void OnEventStartPuzzleButtonClicked(int size)
        {
            EventPuzzleSelected?.Invoke(size);
        }

        private void OnPuzzleLevelClicked(PuzzleLevelButton obj, LevelConfig config)
        {
            _startPuzzlePanel.ShowAsync(new StartPuzzlePanel.StartPuzzlePanel.Info() {Image = config.Image})
                             .Forget();
        }

        public class Info
        {
            public List<LevelConfig> LevelConfigs;
        }
    }
}