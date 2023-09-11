using DG.Tweening;
using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class RotateState : GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {
            context.bow.StartRotationAnimation();
        }

        public override void UpdateState(GameStateManager context)
        {
        }

        public override void EndState(GameStateManager context)
        {
            context.bow.StopBowRotation();
        }
    }
}