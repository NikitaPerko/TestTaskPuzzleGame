using System.Threading;

namespace Runtime.Infrastructure
{
    public class CancellationTokenSourceWrapper
    {
        private CancellationTokenSource _cancellationTokenSource;
        public CancellationToken Token => _cancellationTokenSource.Token;

        public CancellationTokenSourceWrapper()
        {
            Recreate();
        }

        public CancellationTokenSourceWrapper Recreate()
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            return this;
        }

        public CancellationTokenSourceWrapper RecreateLinked(params CancellationToken[] tokens)
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokens);
            return this;
        }

        public void CancelAndDispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
        
        public void CancelAndRecreate()
        {
            Cancel();
            Recreate();
        }

        public void Cancel()
        {
            _cancellationTokenSource?.Cancel();
        }

        public CancellationTokenSourceWrapper CancelAndRecreateLinked(CancellationToken token)
        {
            Cancel();
            RecreateLinked(token);
            return this;
        }
    }
}