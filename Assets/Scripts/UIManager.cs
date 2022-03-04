using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text tilesCountText;
    public GameObject gameOverPanelGO;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void OnClickContinueButton()
    {
        gameOverPanelGO.SetActive(false);
        GameManager.instance.Init();
    }
}
