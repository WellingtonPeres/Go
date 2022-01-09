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
    public Animator textThanks;
    public TextMeshProUGUI numberLevelText;

    [Header("Automatic Count")]
    public int blockCountInScene;

    [Header("Can Start Game")]
    public bool isNextLevel;

    private int actualScene;
    private int lastScenes;

    void Start()
    {
        actualScene = SceneManager.GetActiveScene().buildIndex;
        lastScenes = SceneManager.sceneCountInBuildSettings - 1;

        LoadGame();
        SaveGame("SaveScene", actualScene);

        isNextLevel = false;
        blockCountInScene = FindObjectsOfType<Block>().Length;

        fadeInOut.gameObject.SetActive(true);
        SetTextMeshProUGUI();
        
        StartCoroutine("WaitForStartGame");
    }

    private void SetTextMeshProUGUI()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
            return;
        }
        else
        {
            if (actualScene == lastScenes)
            {
                textThanks.gameObject.SetActive(true);
                textThanks.SetBool("Thanks", true);
                numberLevelText.text = "Thanks for Playing!";
                print("Agradecer!");
                return;
            }

            numberLevelText.text = actualScene.ToString();
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

        if (actualScene == lastScenes)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(actualScene + 1);
        }
    }

    IEnumerator WaitForStartGame()
    {
        yield return new WaitForSeconds(3f);
        isNextLevel = true;
    }

    private void SaveGame(string keyName, int value)
    {
        PlayerPrefs.SetInt(keyName, value);
    }

    private void LoadGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (PlayerPrefs.HasKey("SaveScene"))
            {
                actualScene = PlayerPrefs.GetInt("SaveScene");
                SceneManager.LoadScene(actualScene);
            }
            else
            {
                actualScene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(actualScene + 1);
            }
        }
    }
}
