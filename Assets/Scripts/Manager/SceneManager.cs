using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public int blockCountInScene;

    void Start()
    {
        blockCountInScene = FindObjectsOfType<Block>().Length;
    }

    public void NextLevel(int BlockCountInScene, int AmountBlocksCollider)
    {
        if (BlockCountInScene == AmountBlocksCollider)
        {
            Debug.Log("Próxima fase");
        }
    }
}
