using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class NextLevelState:GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {
            context.GameManager.LoadNextLevel();
        }

        public override void UpdateState(GameStateManager context)
        {
        }

        public override void EndState(GameStateManager context)
        {
        }
    }
}