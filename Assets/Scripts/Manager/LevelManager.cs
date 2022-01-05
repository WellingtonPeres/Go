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

    public bool isNextLevel;
    private int atualScene;

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
        atualScene = SceneManager.GetActiveScene().buildIndex;
        numberLevelText.text = atualScene.ToString();
        numberLevelText.gameObject.SetActive(true);
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

        SceneManager.LoadScene(atualScene + 1);
    }

    IEnumerator WaitForStartGame()
    {
        yield return new WaitForSeconds(4f);
        isNextLevel = true;
    }
}
