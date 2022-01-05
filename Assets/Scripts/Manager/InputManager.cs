using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputData inputData;

    void Update()
    {
        WriteInputData();
        QuitGame();
    }

    private void WriteInputData()
    {
        inputData.isPressed = Input.GetMouseButtonDown(0);
        inputData.isHeld = Input.GetMouseButton(0);
        inputData.isReleased = Input.GetMouseButtonUp(0);
    }

    private void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
