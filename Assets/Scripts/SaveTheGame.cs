using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveTheGame : MonoBehaviour
{
    [NonSerialized] public static int high_score;

    public static void SaveGame()
    {
        PlayerPrefs.SetInt("HighScore", high_score);
        PlayerPrefs.Save();
        Debug.Log("Game data saved!");
    }
}
