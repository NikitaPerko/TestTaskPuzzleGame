using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Features.Game.Ui.StartPuzzlePanel
{
    public class PuzzleSizeButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private int _size;

        public int Size => _size;

        public event Action<PuzzleSizeButton> EventClicked;

        public void Initialize()
        {
            _button.onClick.AddListener(OnButtonClicked);
            SetUnselected();
        }

        public void Deinitialize()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            EventClicked?.Invoke(this);
        }

        public void SetSelected()
        {
            _button.image.color = Color.green;
        }

        public void SetUnselected()
        {
            _button.image.color = Color.white;
        }
    }
}