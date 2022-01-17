using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System.Linq;
using System.Collections;
using System;

public class Main_window_graph_ : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    public bool loaded = false;

    [SerializeField] private List<string> conversion_list;

    private List<Tuple<int, int>> bridge_list;
    private List<Tuple<Method, int>> method_list;
    private List<Tuple<List<double>, int>> graph_list;
    private int parts_count;
    private float high_level;
    private List<Vector2> main_list;

    // Start is called before the first frame update
    void Start()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        high_level = 1f * graphContainer.sizeDelta.y;

        bridge_list = new List<Tuple<int, int>>();
        bridge_list = conversion_list.Select(x =>
        {
            var a = x.Split(',').Select(x => int.Parse(x)).ToList();
            return Tuple.Create(a[0], a[1]);
        }).ToList();

        method_list = new List<Tuple<Method, int>>();
        for (int i = 0; i < conversion_list.Count() / 2; i++)
            method_list.Add(Tuple.Create(Method.Null, i * 2 + 1));

        graph_list = new List<Tuple<List<double>, int>>();
        for (var i = 0; i < method_list.Count(); i++)
        {
            var list_ = new List<double>();
            for (int j = 0; j < bridge_list[method_list[i].Item2].Item1; j++)
                list_.Add(bridge_list[method_list[i].Item2].Item2 * 1.0);
            graph_list.Add(Tuple.Create(list_, method_list[i].Item2));
        }

        transform.parent.Find("Plane").GetComponent<RectTransform>().anchoredPosition
            = new Vector2(26 * graphContainer.sizeDelta.x / 263f + 18, bridge_list[0].Item2 * 0.01f * graphContainer.sizeDelta.y + 7);
        transform.parent.Find("Hole").GetComponent<RectTransform>().anchoredPosition
            = new Vector2((26 + bridge_list.Select(x => x.Item1).Sum()) * graphContainer.sizeDelta.x / 263f + 18, 
                            bridge_list[bridge_list.Count() - 1].Item2 * 0.01f * graphContainer.sizeDelta.y + 7);
        //transform.parent.Find("Settings_panel").gameObject.SetActive(false);

        Configure_parameters();

        transform.parent.Find("Apply_btn").GetComponent<RectTransform>().anchoredPosition = new Vector2(10000, 10000);
        transform.parent.Find("Method_panel").GetComponent<RectTransform>().anchoredPosition = new Vector2(10000, 10000);

        transform.parent.Find("Fixed Joystick").GetComponent<RectTransform>().anchoredPosition = new Vector2(10000, 10000);
        transform.parent.Find("Panel_graph_ctrl_btns").GetComponent<RectTransform>().anchoredPosition = new Vector2(10000, 10000);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Configure_parameters()
    {

        foreach (Transform child in transform.Find("graphContainer"))
            if (child.name != "background")
                Destroy(child.gameObject);

        main_list = Create_main_list(bridge_list);

        ShowGraph(main_list);
    }

    private List<Vector2> Create_main_list(List<Tuple<int, int>> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = 100f;
        float xSize = graphContainer.sizeDelta.x / 263f;
        var result_list = new List<Vector2>();
        var index = 0;

        for (int i = 0; i < valueList.Select(x => x.Item1).Sum(); i++)
        {
            if (i > valueList.Take(index + 1).Select(x => x.Item1).Sum() - 1) index++;
            if (index % 2 == 1)
            {
                result_list = result_list.Concat(Create_list_part(method_list[index / 2].Item1, graphHeight, yMax, xSize, ref i, index)).ToList();
                continue;
            }
            float xPos = 26 * xSize + i * xSize;
            float yPos = ((float)(valueList[index].Item2 / yMax)) * graphHeight;
            result_list.Add(new Vector2(xPos, yPos));
        }

        return result_list;
    }

    private List<Vector2> Create_list_part(Method method, float graphHeight, float yMax, float xSize, ref int i_, int index_)
    {
        var returned_list = new List<Vector2>();
        var graph = graph_list.Where(x => x.Item2 == index_).Select(x => x.Item1).FirstOrDefault();
        Debug.Log(method);

        if (method == Method.Null || method == Method.Parabola)
        {
            foreach (var item in graph)
                returned_list.Add(new Vector2((26 + i_++) * xSize, (float)(item / yMax * graphHeight)));
        }

        if (method == Method.Ellips)
        {
            var y0 = graph[0];
            foreach (var item in graph.Select(x => 2 * y0 - x))
                returned_list.Add(new Vector2((26 + i_++) * xSize, (float)(item / yMax * graphHeight)));
            i_ -= 1;

            foreach (var item in graph)
                returned_list.Add(new Vector2((26 + i_--) * xSize, (float)(item / yMax * graphHeight)));

            foreach (var item in graph.Select(x => 2 * y0 - x))
                returned_list.Add(new Vector2((26 + i_++) * xSize, (float)(item / yMax * graphHeight)));
            i_++;
        }

        if (method == Method.Giperbola)
        {
            if (graph[0] - graph[graph.Count() - 1] > 0)
            {
                var y0 = graph[graph.Count() - 1];
                var a = new List<double>(graph);
                foreach (var item in a.Select(x => 2 * y0 - x))
                    returned_list.Add(new Vector2((26 + i_++) * xSize, (float)(item / yMax * graphHeight)));
                i_ -= 1;
                a.Reverse();
                foreach (var item in a)
                    returned_list.Add(new Vector2((26 + i_--) * xSize, (float)(item / yMax * graphHeight)));
            }
            else
            {
                var y0 = graph[0];
                var a = new List<double>(graph);
                a.Reverse();
                foreach (var item in a)
                    returned_list.Add(new Vector2((26 + i_--) * xSize, (float)(item / yMax * graphHeight)));
                i_++;
                a = a.Select(x => 2 * y0 - x).ToList();
                a.Reverse();
                foreach (var item in a)
                    returned_list.Add(new Vector2((26 + i_++) * xSize, (float)(item / yMax * graphHeight)));
            }
        }
        //Debug.Log(returned_list.Count());
        //Debug.Log("HELLO!!!");
        return returned_list;
    }

    private void ShowGraph(List<Vector2> valueList)
    {
        GameObject lastCircleGameObject = null;

        for (int i = 0; i < valueList.Count; i++)
        {
            GameObject circleGameObject = CreateCircle(valueList[i], unvisible: valueList[i].y > high_level);
            if (lastCircleGameObject != null)
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, 
                    circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            lastCircleGameObject = circleGameObject;

        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, bool unvisible)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        if (unvisible) gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);

        if (Vector2.Distance(dotPosA, dotPosB) > 100f)
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);   
        else
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

    public void Load_Graph_Point(List<double> list, Method _method, int index)
    {
        method_list[index] = Tuple.Create(_method, index * 2 + 1);
        graph_list[index] = Tuple.Create(list, index * 2 + 1);

        Configure_parameters();
    }

    public void Repaint_graph()
    {
        Configure_parameters();
        transform.parent.Find("Plane").GetComponent<Button>().interactable = true;
    }

    public List<Vector2> Points_for_movement()
    {
        return main_list.Select(x => new Vector2(x.x + 18, x.y + 7)).ToList();
    }
}
