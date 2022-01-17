using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private Sprite fullStar;

    private Dictionary<string, List<int>> dict_;
    private bool flag = true;

    // Start is called before the first frame update
    void Start()
    {
        if (flag)
        {
            transform.Find("Settings_panel").gameObject.SetActive(false);
            flag = !flag;
        }

        if (levelName == "Main-menu")
        {
            dict_ = GlobalV.dict_;

            for(int i = 0; i<8; i++)
                SomeFunction(transform.Find("Panel_btn").GetChild(i));

            foreach (Transform child in transform.Find("Panel_btn"))
                child.GetComponent<Button>().interactable = GlobalV.CheckLevel(child.name.Substring(4));

            GlobalV.RefreshHeartsCount();
            //Debug.Log(GlobalV.heartCount);

            transform.Find("Panel_count").Find("Text").GetComponent<Text>().text = GlobalV.heartCount.ToString();
            transform.Find("Settings_panel").Find("Panel").Find("Some_1").GetComponent<Text>().text = GlobalV.heartCount.ToString();

            var a = transform.Find("Panel_characters");
            a.Find("Text").GetComponent<Text>().text = GlobalV.heartCount.ToString() + "/" + GlobalV.maxCount.ToString();
            a.Find("Girl_image").GetComponent<RectTransform>().anchoredPosition =
                new Vector2(-200 + GlobalV.heartCount * 155 / GlobalV.maxCount, 10);
            a.Find("Boy_image").GetComponent<RectTransform>().anchoredPosition =
                new Vector2(200 - GlobalV.heartCount * 155 / GlobalV.maxCount, 10);
        }

        if (levelName.Contains("Scene-Level"))
        {
            var level_ = "level-" + levelName[levelName.Length - 1];
            //Debug.Log(GlobalV.dict_[level_][0]);
            transform.Find("Settings_panel").Find("Panel").Find("Some_1").GetComponent<Text>().text = GlobalV.dict_[level_][0].ToString();

            transform.Find("Finish_panel").Find("Star_percent").GetComponent<Text>().text =
                Mathf.Round(GlobalV.dict_[level_][0] * 100 / GlobalV.dict_[level_][1]).ToString() + "%";
            transform.Find("Finish_panel").Find("Heart_count").GetComponent<Text>().text =
                GlobalV.dict_[level_][0].ToString() + "/" + GlobalV.dict_[level_][1].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SomeFunction(Transform child)
    {
        try
        {
            var a = GlobalV.GetList(child.name.Substring(4));
            var b = Mathf.Round(a[0] * 100 / a[1]);
            child.Find("Percent").GetComponent<Text>().text = b.ToString() + "%";
            if (b > 0) child.Find("Star").GetComponent<Image>().sprite = fullStar;
        }
        catch
        {
            child.Find("Percent").GetComponent<Text>().text = "0%";
        }
    }

    public void Start_()
    {
        Start();
    }
}
