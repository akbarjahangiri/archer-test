
using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class BowPowerChargeState : GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {

            context.HudManager.HandleBowPowerChargeStart();
        }

        public override void UpdateState(GameStateManager context)
        {
        }

        public override void EndState(GameStateManager context)
        {
            context.HudManager.HandleBowPowerChargeEnd();
        }
    }
}
