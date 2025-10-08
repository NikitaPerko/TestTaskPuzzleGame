using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Features.Game.Ui.MainMenu
{
    public class PuzzleLevelButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _image;

        private LevelConfig _config;

        public event Action<PuzzleLevelButton, LevelConfig> EventButtonClicked;

        public void Inititalize(LevelConfig config)
        {
            _config = config;
            _image.sprite = config.Image;
            _button.onClick.AddListener(OnButtonClicked);
        }

        public void Deinitialize()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            EventButtonClicked?.Invoke(this, _config);
        }
    }
}