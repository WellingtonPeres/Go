using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Transition Between Scenes")]
    public Image fadeInOut;
    public Animator imageFadeInOut;
    public TextMeshProUGUI numberLevelText;

    [Header("Automatic Count")]
    public int blockCountInScene;

    [Header("Can Start Game")]
    public bool isNextLevel;
    private int atualScene;
    private int lastScenes;

    private void Awake()
    {
        lastScenes = SceneManager.sceneCountInBuildSettings - 1;
    }

    void Start()
    {
        isNextLevel = false;
        blockCountInScene = FindObjectsOfType<Block>().Length;

        fadeInOut.gameObject.SetActive(true);
        SetTextMeshProUGUI();

        StartCoroutine("WaitForStartGame");
    }

    private void SetTextMeshProUGUI()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }
        else
        {
            atualScene = SceneManager.GetActiveScene().buildIndex;
            numberLevelText.text = atualScene.ToString();
            numberLevelText.gameObject.SetActive(true);
        }
    }

    public void NextLevel(int BlockCountInScene, int AmountBlocksCollider)
    {
        if (BlockCountInScene == AmountBlocksCollider)
        {
            StartCoroutine("WaitForNextLevel");
        }
    }

    IEnumerator WaitForNextLevel()
    {
        imageFadeInOut.SetTrigger("FadeOut");

        yield return new WaitForSeconds(2);

        if (atualScene == lastScenes)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(atualScene + 1);
        }
    }

    IEnumerator WaitForStartGame()
    {
        yield return new WaitForSeconds(4f);
        isNextLevel = true;
    }
}

//    //if (PlayerPrefs.HasKey("scenes"))
//    //{
//    //    scenesIndex = PlayerPrefs.GetInt("scenes");
//    //}

//    //PlayerPrefs.SetInt("scenes", scenesIndex);
