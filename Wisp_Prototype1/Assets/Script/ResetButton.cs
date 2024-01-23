using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    // Method to reset the game by reloading the current scene
    public void OnResetButtonPressed()
    {
        // Reloads the current scene to reset the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}