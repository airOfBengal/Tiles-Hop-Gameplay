using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static long tilesCount = 0;
    public static long diamondCount = 0;
    public static long starCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        diamondCount = long.Parse(PlayerPrefs.GetString("diamond", "0"));
        starCount = long.Parse(PlayerPrefs.GetString("star", "0"));
        tilesCount = long.Parse(PlayerPrefs.GetString("tiles", "0"));
        UIManager.instance.tilesCountText.text = tilesCount.ToString();
    }

 
    public static void SaveTilesCount()
    {
        PlayerPrefs.SetString("tiles", tilesCount.ToString());
        PlayerPrefs.Save();
    }

    public static void SaveDiamondCount()
    {
        PlayerPrefs.SetString("diamond", diamondCount.ToString());
        PlayerPrefs.Save();
    }

    public static void SaveStarCount()
    {
        PlayerPrefs.SetString("star", starCount.ToString());
        PlayerPrefs.Save();
    }
}
