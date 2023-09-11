namespace _Scripts.StateMachine
{
    public abstract class GameBaseState
    {
        public abstract void EnterState(GameStateManager context);
        public abstract void UpdateState(GameStateManager context);
        public abstract void EndState(GameStateManager context);
    }
}