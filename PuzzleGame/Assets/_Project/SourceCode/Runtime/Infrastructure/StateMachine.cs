using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Infrastructure
{
    public class StateMachine
    {
        private List<BaseState> _states;

        private BaseState _currentState;

        public void SetStates(List<BaseState> states)
        {
            _states = states;
        }

        public void SetCurrentState<T>()
            where T : BaseStateWithoutData
        {
            _currentState?.OnExit();
            var state = _states.Find(x => x is T);
            if (state == null)
            {
                throw new System.Exception($"State of type {typeof(T).FullName} not found");
            }

            _currentState = state;
            if (_currentState is BaseStateWithoutData stateWithData)
            {
                stateWithData.OnEnter();
            }
            else
            {
                Debug.LogError("State of type " + _currentState.GetType().FullName + " is not state without data");
            }
        }

        public void SetCurrentStateWithData<T, TData>(TData data = null)
            where T : BaseStateWithData<TData>
            where TData : class
        {
            _currentState.OnExit();
            var state = _states.Find(x => x is T);
            if (state == null)
            {
                throw new System.Exception($"State of type {typeof(T).FullName} not found");
            }

            _currentState = state;
            if (_currentState is BaseStateWithData<TData> stateWithData)
            {
                stateWithData.OnEnter(data);
            }
            else
            {
                Debug.LogError("State of type " + _currentState.GetType().FullName + " is not state with data");
            }
        }
    }
}