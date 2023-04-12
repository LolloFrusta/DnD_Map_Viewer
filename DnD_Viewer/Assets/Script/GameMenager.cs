using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public static class GameMenager
{
    private static bool gameIsInPause = false;
    private static GameObject selected_character;
    public static string selectable_tag = "Selectable";
    public static GameObject CurrentSelectedCharacter { get => selected_character; set => selected_character = value; }

    public static bool GetGameIsPaused()
    {
        return gameIsInPause;
    }
    public static void SetGameIsPaused(bool pause)
    {

        gameIsInPause = pause;
    }

}
