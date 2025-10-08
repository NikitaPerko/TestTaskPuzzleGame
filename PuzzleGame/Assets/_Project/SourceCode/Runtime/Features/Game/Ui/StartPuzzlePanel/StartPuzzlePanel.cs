using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Runtime.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Features.Game.Ui.StartPuzzlePanel
{
    public class StartPuzzlePanel : MonoBehaviour, IUiElement<StartPuzzlePanel.Info>
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private List<PuzzleSizeButton> _sizeButtons;

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _freeStartButton;

        [SerializeField]
        private Button _paidStartButton;

        [SerializeField]
        private Button _advertisementStartButton;

        private PuzzleSizeButton _currentSizeButton;

        private readonly CancellationTokenSourceWrapper _openCloseTokenSource = new();

        public event Action<int, StartPuzzleType> EventStartPuzzleButtonClicked;

        public void Show(Info showInfo)
        {
            _currentSizeButton = null;
            _openCloseTokenSource.CancelAndDispose();
            gameObject.SetActive(true);
            foreach (var sizeButton in _sizeButtons)
            {
                sizeButton.Initialize();
                sizeButton.EventClicked += OnSizeButtonClicked;
            }

            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _freeStartButton.onClick.AddListener(OnFreeStartButtonClicked);
            _paidStartButton.onClick.AddListener(OnPaidStartButtonClicked);
            _advertisementStartButton.onClick.AddListener(OnAdvertisementStartButtonClicked);
            _canvasGroup.alpha = 1f;
            _image.sprite = showInfo.Image;
        }

        public void Hide()
        {
            _openCloseTokenSource.CancelAndDispose();
            foreach (var sizeButton in _sizeButtons)
            {
                sizeButton.Deinitialize();
                sizeButton.EventClicked -= OnSizeButtonClicked;
            }

            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _freeStartButton.onClick.RemoveListener(OnFreeStartButtonClicked);
            _paidStartButton.onClick.RemoveListener(OnPaidStartButtonClicked);
            _advertisementStartButton.onClick.RemoveListener(OnAdvertisementStartButtonClicked);
            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public async UniTask ShowAsync(Info info, CancellationToken cancellationToken = default)
        {
            _openCloseTokenSource.CancelAndRecreateLinked(cancellationToken);

            gameObject.SetActive(true);
            _image.sprite = info.Image;
            foreach (var sizeButton in _sizeButtons)
            {
                sizeButton.Initialize();
                sizeButton.EventClicked += OnSizeButtonClicked;
            }

            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _freeStartButton.onClick.AddListener(OnFreeStartButtonClicked);
            _paidStartButton.onClick.AddListener(OnPaidStartButtonClicked);
            _advertisementStartButton.onClick.AddListener(OnAdvertisementStartButtonClicked);

            await Sequence.Create()
                          .Chain(Tween.Alpha(_canvasGroup, 1f, 0.5f))
                          .ToUniTask(cancellationToken: _openCloseTokenSource.Token).SuppressCancellationThrow();
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            var token = _openCloseTokenSource.CancelAndRecreateLinked(cancellationToken).Token;

            foreach (var sizeButton in _sizeButtons)
            {
                sizeButton.Deinitialize();
                sizeButton.EventClicked -= OnSizeButtonClicked;
            }

            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _freeStartButton.onClick.RemoveListener(OnFreeStartButtonClicked);
            _paidStartButton.onClick.RemoveListener(OnPaidStartButtonClicked);
            _advertisementStartButton.onClick.RemoveListener(OnAdvertisementStartButtonClicked);

            await Sequence.Create()
                          .Chain(Tween.Alpha(_canvasGroup, 0f, 0.5f))
                          .ToUniTask(cancellationToken: token).SuppressCancellationThrow();

            gameObject.SetActive(false);
        }

        private void OnSizeButtonClicked(PuzzleSizeButton button)
        {
            foreach (var sizeButton in _sizeButtons)
            {
                if (ReferenceEquals(sizeButton, button))
                {
                    sizeButton.SetSelected();
                }
                else
                {
                    sizeButton.SetUnselected();
                }
            }

            _currentSizeButton = button;
        }

        private void OnCloseButtonClicked()
        {
            HideAsync().Forget();
        }

        private void OnFreeStartButtonClicked()
        {
            if (_currentSizeButton == null)
            {
                return;
            }

            EventStartPuzzleButtonClicked?.Invoke(_currentSizeButton.Size, StartPuzzleType.Free);
        }

        private void OnPaidStartButtonClicked()
        {
            if (_currentSizeButton == null)
            {
                return;
            }

            EventStartPuzzleButtonClicked?.Invoke(_currentSizeButton.Size, StartPuzzleType.Paid);
        }

        private void OnAdvertisementStartButtonClicked()
        {
            if (_currentSizeButton == null)
            {
                return;
            }

            EventStartPuzzleButtonClicked?.Invoke(_currentSizeButton.Size, StartPuzzleType.ForAd);
        }

        public class Info
        {
            public Sprite Image;
        }
    }
}