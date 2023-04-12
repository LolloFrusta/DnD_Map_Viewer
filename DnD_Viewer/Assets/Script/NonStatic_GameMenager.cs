using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NonStatic_GameMenager : MonoBehaviour
{
    public static void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void OpenFolder()
    {
        System.Diagnostics.Process.Start(Application.dataPath);
    }
}
