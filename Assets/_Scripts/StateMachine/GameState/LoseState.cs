
using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class LoseState : GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {
            context.HudManager.HandleLose();
            Debug.Log("you lose!");
        }

        public override void UpdateState(GameStateManager context)
        {
        }

        public override void EndState(GameStateManager context)
        {
        }
    }
}
