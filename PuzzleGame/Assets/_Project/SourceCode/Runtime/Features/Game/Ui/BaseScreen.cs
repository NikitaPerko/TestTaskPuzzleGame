using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Features.Game.Ui
{
    public abstract class BaseScreen<T> : MonoBehaviour, IUiElement<T>
    {
        public virtual void Show(T showInfo = default)
        {
        }

        public virtual void Hide()
        {
        }

        public async virtual UniTask ShowAsync(T info = default, CancellationToken cancellationToken = default)
        {
        }

        public async virtual UniTask HideAsync(CancellationToken cancellationToken = default)
        {
        }
    }
}