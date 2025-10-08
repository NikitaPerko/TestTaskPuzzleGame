using System.Collections.Generic;
using Runtime.Features.ProjectStates;
using Runtime.Infrastructure;
using UnityEngine;

namespace Runtime
{
    public class ProjectInitializer : MonoBehaviour
    {
        [SerializeField]
        private ProjectReferences _references;

        private StateMachine _stateMachine;

        private void Start()
        {
            _stateMachine = new StateMachine();

            _stateMachine.SetStates(new List<BaseState>
            {
                new MainMenuState(_stateMachine, _references.MainMenuScreen, _references.LevelConfigs),
                new GameState()
            });

            _stateMachine.SetCurrentState<MainMenuState>();
        }
    }
}