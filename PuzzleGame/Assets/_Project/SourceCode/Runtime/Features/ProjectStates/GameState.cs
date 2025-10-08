using Runtime.Infrastructure;

namespace Runtime.Features.ProjectStates
{
    public class GameState : BaseStateWithData<GameState.GameStateData>
    {
        public override void OnEnter(GameStateData data)
        {
            
        }

        public class GameStateData
        {
            public int Size;
        }
    }
}