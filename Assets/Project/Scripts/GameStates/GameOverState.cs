public class GameOverState : IGameState
{
    GameEvent uIOnly;

    public GameOverState(GameEvent uIOnlyRequest) => uIOnly = uIOnlyRequest;

    public void Enter() => uIOnly.Raise();
}