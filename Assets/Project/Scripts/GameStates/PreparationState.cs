public class PreparationState : IGameState
{
    GameEvent inGameOnly;

    public PreparationState(GameEvent inGameOnlyRequest) => inGameOnly = inGameOnlyRequest;
    
    public void Enter() => inGameOnly.Raise();
}