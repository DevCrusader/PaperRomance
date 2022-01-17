using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePrefs : MonoBehaviour
{
    public void SaveGame()
    {
        PlayerPrefs.SetString("SavedLevelParams", string.Join("|", GlobalV.dict_.Values.Select(x => x[0]).ToList()));
        PlayerPrefs.SetString("SavedCompleteLevels", string.Join("|", GlobalV.list_));
        PlayerPrefs.Save();
        Debug.Log("Game data saved!");
        SceneManager.LoadScene("Main-menu");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SavedLevelParams"))
        {
            var a = PlayerPrefs.GetString("SavedLevelParams").Split('|');
            for (var i = 0; i < a.Count(); i++)
                GlobalV.SetHeartCount("level-" + (i + 1).ToString(), int.Parse(a[i]));
            foreach (var c in PlayerPrefs.GetString("SavedCompleteLevels").Split('|'))
                GlobalV.AddLevel(c);
            SceneManager.LoadScene("Main-menu");
        }
        else
            Debug.Log("No loaded data");
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Data reset complete");
    }
}
