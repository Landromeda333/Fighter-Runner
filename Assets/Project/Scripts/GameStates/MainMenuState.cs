using UnityEngine;

public class MainMenuState : IGameState
{
    GameEvent uiOnly;

    public MainMenuState(GameEvent uiOnlyRequest) => uiOnly = uiOnlyRequest;

    public void Enter()
    {
        Time.timeScale = 1;
        uiOnly.Raise();
    }
}