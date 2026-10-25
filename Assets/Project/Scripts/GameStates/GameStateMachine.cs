public class GameStateMachine
{
    IGameState currentState;

    public void ChangeState(IGameState newState)
    {
        currentState = newState;
        currentState.Enter();
    }
}