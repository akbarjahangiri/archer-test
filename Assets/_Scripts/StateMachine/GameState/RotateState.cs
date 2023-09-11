using DG.Tweening;
using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class RotateState : GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {
            Debug.Log("Bow Rotate State");
            context.bow.StartRotationAnimation();
        }

        public override void UpdateState(GameStateManager context)
        {
        }

        public override void EndState(GameStateManager context)
        {
            Debug.Log("Bow Rotate State End");
            context.bow.StopBowRotation();
        }
    }
}