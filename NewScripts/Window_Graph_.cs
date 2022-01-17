using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class Window_Graph_: MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private int highPoint;
    [SerializeField] private int downPoint;
    [SerializeField] private int rightPoint;
    [SerializeField] private int leftPoint;
    [SerializeField] private int width;

    public bool choosedPanel = false;
    public bool ready = false;
    public Method method = Method.Null;

    private bool change = false;
    private Text textContainer;
    private RectTransform graphContainer;
    private bool flag = true;

    private double x_pass = 0;
    private double x0;

    private double k = 1;
    private double a = 0;
    private double b = 50;

    private double c_1 = 1000;
    private double c_2 = 100;
    private double y0 = 50;
    private double max_c_1;

    private double d_1 = 1000;
    private double d_2 = 100;
    private double y0_g = 50;

    private List<double> valueList = new List<double>();

    // Start is called before the first frame update
    void Start()
    {
        textContainer = transform.parent.Find("Apply_btn").Find("Text").GetComponent<Text>();
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();

        a = graphContainer.sizeDelta.x / 2;
        x0 = graphContainer.sizeDelta.x / 2;
        max_c_1 = 0.000249 * graphContainer.sizeDelta.x * graphContainer.sizeDelta.x;
        Debug.Log(max_c_1);

        transform.Find("background").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (Math.Abs(valueList[0] - leftPoint) <= 2 && Math.Abs(valueList[valueList.Count - 1] - rightPoint) <= 2)
            {
                if (method is Method.Parabola)
                    ready = true;
                if (method is Method.Ellips)
                    ready = true;
            }

            if (method is Method.Giperbola && Math.Abs(valueList[0] - highPoint) <= 2 && Math.Abs(2 * y0_g - valueList[0] - downPoint) <= 2
                && Math.Abs(valueList[valueList.Count - 1] - highPoint) <= 2 && Math.Abs(2 * y0_g - valueList[valueList.Count - 1] - downPoint) <= 2)
                ready = true;
            else if (method is Method.Giperbola) throw new IndexOutOfRangeException();
        }
        catch { ready = false; };
    }


    private void Configure_parameters()
    {
        textContainer.text = "МЕСТО ДЛЯ ФОРМУЛЫ";

        foreach (Transform child in transform.Find("graphContainer"))
            if (child.name != "background") Destroy(child.gameObject);

        if (choosedPanel || change)
        {
            if (method is Method.Parabola)
            {
                valueList = CreateParabolaList();
                textContainer.text = String.Join("", new List<string> { "y = ", k.ToString(), 
                    " * ( x + (", Math.Round((2*a/graphContainer.sizeDelta.x - 1) * 37).ToString(), 
                    "))² + (", (b - 50).ToString(), ")"});

                ShowGraph(valueList);
            }

            if (method is Method.Ellips)
            {
                valueList = CreateEllipsList();
                textContainer.text = String.Join("", new List<string> { "(x - (", 
                    Math.Round(((2*x0 + x_pass * graphContainer.sizeDelta.x / width * 1f)/graphContainer.sizeDelta.x - 1) * 74).ToString(),
                    "))² / ", (c_1 / 1000).ToString(), "² + (y - (",  (y0 - 50).ToString(),
                    "))² / ", (c_2 / 100).ToString(), "² = 1"});

                ShowGraph(valueList);
                ShowGraph(valueList.Select(x => 2 * y0 - x).ToList());
            }

            if (method is Method.Giperbola)
            {
                valueList = CreateGiperbolaList();
                textContainer.text = String.Join("", new List<string> { "(x - (",
                    Math.Round(((2*x0 + x_pass * graphContainer.sizeDelta.x / width * 1f)/graphContainer.sizeDelta.x - 1) * 74).ToString(),
                    "))² / ", (d_1 / 1000).ToString(), "² - (y - (",  (y0_g - 50).ToString(),
                    "))² / ", (d_2 / 100).ToString(), "² = 1"});

                ShowGraph(valueList);
                ShowGraph(valueList.Select(x => 2 * y0_g - x).ToList());
            }
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<double> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float yMax = 100f;
        float xSize = graphWidth / width * 1f;
        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = i * xSize + xSize * (float)x_pass;
            float yPos = ((float)(valueList[i] / yMax)) * graphHeight;
            if (yPos < 0 || yPos > graphHeight) continue;
            if (xPos < 0 || xPos > graphWidth) continue;
            GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos));
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPosB - dotPosA).normalized;
        float distance = Vector2.Distance(dotPosA, dotPosB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPosA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    private List<double> CreateParabolaList()
    {
        var reslist = new List<double>();
        var sd_x = graphContainer.sizeDelta.x / width * 1f;
        for (float x = 0; x <= graphContainer.sizeDelta.x; x += sd_x)
        {
            var num = k * (x - a) * (x - a);
            reslist.Add(num / 100 + b);
        }
        x_pass = 0;
        return reslist;
    }

    private List<double> CreateEllipsList()
    {
        var result = new List<double>();
        var sd_x = graphContainer.sizeDelta.x / width * 1f;
        var flag = true;
        int index = 0;

        for (float x = 0; x < graphContainer.sizeDelta.x; x += sd_x)
        {
            var num = c_2 - (x - x0) * (x - x0) * c_2/ c_1;
            if (num >= 0)
            {
                result.Add(Math.Sqrt(num) + y0);
                if (flag && result[result.Count - 1] != y0)
                {
                    index = result.Count - 1;
                    result.Insert(index, y0);
                    flag = false;
                }
            }
            else result.Add(double.NaN);
        }
        index = result.Count - index;
        if (result[index] != y0) result.Insert(index, y0);
        result.RemoveAt(0);
        result.RemoveAt(result.Count - 1);

        return result;
    }

    private List<double> CreateGiperbolaList()
    {
        var result = new List<double>();
        var sd_x = graphContainer.sizeDelta.x / width * 1f;

        for (float x = 0; x < graphContainer.sizeDelta.x; x += sd_x)
        {
            var num = (x - x0) * (x - x0) * d_2 / d_1 - d_2;
            if (num >= 0)
            {
                result.Add(Math.Sqrt(num) + y0_g);
            }
            else result.Add(double.NaN);
        }

        var index = result.IndexOf(double.NaN);

        result.RemoveAt(index);
        result.Insert(index, y0_g);
        result.RemoveAt(result.Count - 1 - index);
        result.Insert(result.Count - index, y0_g);

        return result;
    }

    public void Change_choosed_bool(bool change_)
    {
        choosedPanel = change_;

        transform.Find("background").gameObject.SetActive(change_ || change);
        if (change_)
        {
            //transform.Find("Btn_choosed_panel").gameObject.SetActive(false);
            transform.Find("Btn_choosed_panel").GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            transform.Find("Btn_choosed_panel").Find("Image").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f); ;
        }
        else
        {
            var a = transform.Find("Btn_choosed_panel").GetComponent<Button>().colors;
            a.normalColor = new Color(1, 1, 1, 1);
            transform.Find("Btn_choosed_panel").Find("Image").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        if (flag)
        {
            Change_method(Method.Parabola);
            flag = !flag;
        }

        Configure_parameters();
    }

    public void Up_y()
    {
        if (method == Method.Parabola) b += 2.5;
        if (method == Method.Ellips) y0 += 2.5;
        if (method == Method.Giperbola) y0_g += 2.5;
        change = true;
        Configure_parameters();
    }
    public void Down_y()
    {
        if (method == Method.Parabola) b -= 2.5;
        if (method == Method.Ellips) y0 -= 2.5;
        if (method == Method.Giperbola) y0_g -= 2.5;
        change = true;
        Configure_parameters();
    }

    public void Right_x()
    {
        if (method == Method.Parabola) a += 10;
        if (method == Method.Ellips) x_pass += 3;
        if (method == Method.Giperbola) x_pass += 3;
        change = true;
        Configure_parameters();
    }
    public void Left_x()
    {
        if (method == Method.Parabola) a -= 10;
        if (method == Method.Ellips) x_pass -= 3;
        if (method == Method.Giperbola) x_pass -= 3;
        change = true;
        Configure_parameters();
    }

    public void Change_coef(double delta, int index = 0)
    {
        if (index == 0 && method == Method.Parabola)
        {
            if (Math.Abs(k + delta) <= 0.05) k = -k;
            k += delta;
            k = Math.Round(k, 3);
        }
        if (index == 1 && method == Method.Ellips) if (c_1 + delta * 1000 > 0 && (c_1 + delta * 1000) / 1000 <= max_c_1) c_1 += delta * 1000;
        if (index == 2 && method == Method.Ellips) if (c_2 + delta * 1000 > 0) c_2 += delta * 1000;

        if (index == 1 && method == Method.Giperbola) if (d_1 + delta * 1000 > 0) d_1 += delta * 1000;
        if (index == 2 && method == Method.Giperbola) if (d_2 + delta * 1000 > 0) d_2 += delta * 1000;
        change = true;
        Configure_parameters();
    }

    public void Change_method(Method new_method)
    {
        method = new_method;
        change = false;
        x_pass = 0;
        Configure_parameters();
    }

    public List<double> Apply_command()
    {
        transform.gameObject.SetActive(false);
        transform.parent.Find("Apply_btn").gameObject.SetActive(false);
        transform.parent.Find("Method_panel").gameObject.SetActive(false);

        if (method == Method.Giperbola)
            return valueList.Take(valueList.IndexOf(double.NaN)).ToList();

        return valueList;
    }
}


public enum Method
{
    Null,
    Parabola,
    Ellips,
    Giperbola
}