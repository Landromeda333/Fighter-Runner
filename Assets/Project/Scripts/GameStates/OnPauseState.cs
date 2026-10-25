public class OnPauseState : IGameState
{
    GameEvent uIOnly;

    public OnPauseState(GameEvent uIOnlyRequest) => uIOnly = uIOnlyRequest;

    public void Enter() => uIOnly.Raise();
}