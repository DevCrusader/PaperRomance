using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph_ctrl_with_btn_ : MonoBehaviour
{
    private string cur_graph;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set_current_graph(string name) => cur_graph = name;

    public void Ctrl_up_y()
    {
        var graph = transform.Find(cur_graph).GetComponent<Window_Graph_>();
        if (graph.choosedPanel) graph.Up_y();
    }

    public void Ctrl_down_y()
    {
        var graph = transform.Find(cur_graph).GetComponent<Window_Graph_>();
        if (graph.choosedPanel) graph.Down_y();
    }

    public void Ctrl_rignt_x()
    {
        var graph = transform.Find(cur_graph).GetComponent<Window_Graph_>();
        if (graph.choosedPanel) graph.Right_x();
    }
    public void Ctrl_left_x()
    {
        var graph = transform.Find(cur_graph).GetComponent<Window_Graph_>();
        if (graph.choosedPanel) graph.Left_x();
    }

    public void Change_method(string method)
    {
        //Debug.Log(cur_graph);
        var graph = transform.Find(cur_graph).GetComponent<Window_Graph_>();
        if (graph.choosedPanel)
        {
            switch (method)
            {
                case "parabola":
                    {
                        graph.Change_method(Method.Parabola);
                        return;
                    }
                case "ellips":
                    {
                        graph.Change_method(Method.Ellips);
                        return;
                    }
                case "giperbola":
                    {
                        graph.Change_method(Method.Giperbola);
                        return;
                    }

            }
        }
    }
}
