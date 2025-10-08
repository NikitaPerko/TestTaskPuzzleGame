using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Features.Game.Ui
{
    public interface IUiElement<T>
    {
        public void Show(T showInfo = default);
        public void Hide();
        public UniTask ShowAsync(T showInfo = default, CancellationToken cancellationToken = default);
        public UniTask HideAsync(CancellationToken cancellationToken = default);
    }
}