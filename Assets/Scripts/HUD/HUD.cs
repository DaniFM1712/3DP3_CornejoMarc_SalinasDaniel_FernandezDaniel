using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Text score;
    [SerializeField] float timeToSetHudInvisible;
    bool hudIsVisible = true;
    float lastTimeHudWasVisible;
    private void Start()
    {
        IScoreManager l_IScoreManager = GameControllerScript.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>();
        l_IScoreManager.scoreChangedDelegate += updateScore;
        updateScore(l_IScoreManager);
        lastTimeHudWasVisible = Time.time;
    }

    private void Update()
    {
        if (hudIsVisible)
        {
            if (Time.time >= timeToSetHudInvisible + lastTimeHudWasVisible)
            {
                MakeHudInvisible();
                lastTimeHudWasVisible = Time.time;
            }
        }
    }
    private void OnDestroy()
    {
        GameControllerScript.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>().scoreChangedDelegate -= updateScore;
    }
    public void updateScore(IScoreManager scoreManager)
    {
        score.text = scoreManager.getPoints().ToString("0");
    }

    public void MakeHudVisible()
    {
        gameObject.GetComponent<Animation>().CrossFade("EnterUIAnimation");
        hudIsVisible = true;
        lastTimeHudWasVisible = Time.time;
    }
    public void MakeHudInvisible()
    {
        gameObject.GetComponent<Animation>().CrossFade("ExitUIAnimation");
        hudIsVisible = false;
    }
}
