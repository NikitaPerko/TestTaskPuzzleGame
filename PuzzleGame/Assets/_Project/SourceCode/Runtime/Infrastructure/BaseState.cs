namespace Runtime.Infrastructure
{
    public class BaseStateWithData<TData> : BaseState where TData : class
    {
        public virtual void OnEnter(TData data)
        {
        }
    }

    public class BaseStateWithoutData : BaseState
    {
        public virtual void OnEnter()
        {
        }
    }

    public abstract class BaseState
    {
        public virtual void OnExit()
        {
        }
    }
}