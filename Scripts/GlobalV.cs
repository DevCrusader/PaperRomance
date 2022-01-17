using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalV : MonoBehaviour
{
    public static int heartCount = 0;
    public static int maxCount = 47;
    public static Dictionary<string, List<int>> dict_ = new Dictionary<string, List<int>>();
    public static List<string> list_ = new List<string> { "level-1" };

    public static void InitializeDictionary()
    {
        dict_.Add("level-1", new List<int> { 0, 3 });
        dict_.Add("level-2", new List<int> { 0, 4 });
        dict_.Add("level-3", new List<int> { 0, 4 });
        dict_.Add("level-4", new List<int> { 0, 4 });
        dict_.Add("level-5", new List<int> { 0, 7 });
        dict_.Add("level-6", new List<int> { 0, 7 });
        dict_.Add("level-7", new List<int> { 0, 8 });
        dict_.Add("level-8", new List<int> { 0, 10 });
    }

    public static List<int> GetList(string key)
    {
        return dict_[key];
    } 

    public static void AddLevel(string level)
    {
        if (!list_.Contains(level)) list_.Add(level);
    }

    public static bool CheckLevel(string level)
    {
        return list_.Contains(level);
    }

    public static void RefreshHeartsCount()
    {
        var sum = 0;
        foreach(var value in dict_.Values)
            sum += value[0];

        heartCount = sum;
    }

    public static void SetHeartCount(string level, int count)
    {
        dict_[level][0] = count;
    }
}
