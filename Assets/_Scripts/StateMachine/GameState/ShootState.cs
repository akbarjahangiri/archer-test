using UnityEngine;

namespace _Scripts.StateMachine.GameState
{
    public class ShootState : GameBaseState
    {
        private float _timer = 0f;
        private float _transitionTime = 2f;

        public override void EnterState(GameStateManager context)
        {
            context.bow.HandleShootState();
        }

        public override void UpdateState(GameStateManager context)
        {
            _timer += Time.deltaTime;
            if (_timer >= _transitionTime)
            {
                context.SwitchState(context.IdleState);
            }
        }

        public override void EndState(GameStateManager context)
        {
            _timer = 0f;
            _transitionTime = 2f;
        }
    }
}