using System;
using _Scripts.StateMachine.GameState;
using Archer.Managers;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public class GameStateManager:MonoBehaviour
    {
        public GameBaseState currentState;
        public IdleState IdleState = new IdleState();
        public RotateState RotateState = new RotateState();
        public BowPowerChargeState BowPowerChargeState = new BowPowerChargeState();
        public ShootState ShootState = new ShootState();
        public CheckArrowState CheckArrowState = new CheckArrowState();
        public ReloadArrowState ReloadArrowState = new ReloadArrowState();
        public EmptyQuiverState EmptyQuiverState = new EmptyQuiverState();
        public NextLevelState NextLevelState = new NextLevelState();
        public VictoryState VictoryState = new VictoryState();
        public LoseState LoseState = new LoseState();
        

        public Bow bow;
        public HudManager HudManager;
        public GameManager GameManager;
        private void Start()
        {
            Debug.Log("Start gameStateManager");
            currentState = IdleState;
            currentState.EnterState(this);
        }

        private void Update()
        {
            currentState.UpdateState(this);
        }

        public void SwitchState(GameBaseState state)
        {
            currentState.EndState(this);
            currentState = state;
            state.EnterState(this);
        }
    }
}