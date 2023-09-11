using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class EmptyQuiverState:GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {
            Debug.Log("Empty Quiver State");
            if (context.GameManager.CheckPlayerProgress())
            {
                if (context.GameManager.CheckLastLevel())
                {
                    Debug.Log("You win!");
                    context.SwitchState(context.VictoryState);
                }
                else
                {
                    context.SwitchState(context.NextLevelState);
                }
            }
            else
            {
                context.SwitchState(context.LoseState);

            }
        }

        public override void UpdateState(GameStateManager context)
        {
        }

        public override void EndState(GameStateManager context)
        {
        }
    }
}