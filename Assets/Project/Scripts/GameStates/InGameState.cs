public class InGameState : IGameState
{
    GameEvent inGameOnly;

    public InGameState(GameEvent inGameOnlyRequest) => inGameOnly = inGameOnlyRequest;

    public void Enter() => inGameOnly.Raise();
}