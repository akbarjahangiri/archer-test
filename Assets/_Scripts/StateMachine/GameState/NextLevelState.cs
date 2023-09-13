using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class NextLevelState:GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {
            context.HudManager.StartloadingNextLevelEffect();
        }

        public override void UpdateState(GameStateManager context)
        {
            if (context.HudManager.CheckloadingNextLevelEffectStatus())
            {
                context.GameManager.LoadNextLevel();
            }
        }

        public override void EndState(GameStateManager context)
        {
            context.HudManager.ResetloadingNextLevelEffectStatus();
        }
    }
}