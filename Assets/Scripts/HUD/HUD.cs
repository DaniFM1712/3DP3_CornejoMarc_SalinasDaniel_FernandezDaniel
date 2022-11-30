using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text score;
    private void Start()
    {
        IScoreManager l_IScoreManager = GameControllerScript.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>();
        l_IScoreManager.scoreChangedDelegate += updateScore;
        updateScore(l_IScoreManager);
    }
    private void OnDestroy()
    {
        GameControllerScript.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>().scoreChangedDelegate -= updateScore;
    }
    public void updateScore(IScoreManager scoreManager)
    {
        score.text = scoreManager.getPoints().ToString("0");
    }
}
