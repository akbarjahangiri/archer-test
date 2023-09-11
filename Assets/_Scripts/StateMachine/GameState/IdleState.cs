

using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class IdleState : GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {
            if (context.bow.HandleCheckArrow())
            {
                context.HudManager.HandleIdleState();
                context.bow.HandleReloadArrow();
            }
            else
            {
                context.SwitchState(context.EmptyQuiverState);
            }
        }

        public override void UpdateState(GameStateManager context)
        {
            //context.SwitchState(context.RotateState);
        }

        public override void EndState(GameStateManager context)
        {
            Debug.Log("Idle State Change");
        }
    }
}
