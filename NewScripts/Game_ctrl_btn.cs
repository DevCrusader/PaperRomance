using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_ctrl_btn : MonoBehaviour
{
    //[SerializeField] private Window_Graph_ window_graph;
    //private string cur_graph;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("Apply_btn").GetComponent<Button>().interactable = Check_ready();
            
    }

    private bool Check_ready()
    {
        foreach (Transform child in transform)
            if (child.name.Contains("window_graph_")) 
                if (!child.GetComponent<Window_Graph_>().ready) return false;
        return true;
    }
    //public void Set_current_graph(string name) => cur_graph = name;

    public void Apply_btn()
    {
        var main = transform.Find("main_window_graph").GetComponent<Main_window_graph_>();
        var index = 0;
        foreach (Transform child in transform)
        {
            if (child.name.Contains("window_graph_"))
            {
                var graph = child.GetComponent<Window_Graph_>();
                Debug.Log(graph.method);
                main.Load_Graph_Point(graph.Apply_command(), graph.method, index++);

                if (graph.method == Method.Giperbola)
                {
                    var a = graph.Apply_command();
                    a.Reverse();
                    main.Load_Graph_Point(a, graph.method, index++);
                }
            }
        }
        main.Repaint_graph();

        transform.Find("Plane").GetComponent<Start_plane_move_>().Start_move();
    }
}
