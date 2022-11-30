using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreManager, IRestartGameElement
{
    [SerializeField] float points;
    public event ScoreChanged scoreChangedDelegate;
    void Awake()
    {
        GameControllerScript.GetGameController().GetDependencyInjector().AddDependency<IScoreManager>(this);
        GameControllerScript.GetGameController().AddRestartGameElement(this);
    }
    public void addPoints(float points)
    {
        this.points += points;
        scoreChangedDelegate?.Invoke(this);
    }
    public float getPoints() { return points; }

    public void RestartGame()
    {
        IScoreManager scoreManager = GameControllerScript.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>();
        scoreManager.addPoints(-scoreManager.getPoints());
    }
}
