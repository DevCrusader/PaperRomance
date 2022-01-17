using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph_ctrl_joystick_ : MonoBehaviour
{
    private string cur_graph = "window_graph_1";
    [SerializeField] private Joystick joystick;
    private int frameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Set_current_graph(string name) => cur_graph = name;

    // Update is called once per frame
    void Update()
    {
        var graph = transform.parent.Find(cur_graph).GetComponent<Window_Graph_>();
        if (frameCount % 10 == 0 && graph.choosedPanel)
        {
            if (graph.method == Method.Parabola)
            { 
                var delta = Mathf.Abs(joystick.Vertical) > Mathf.Abs(joystick.Horizontal) ?
                   joystick.Vertical : joystick.Horizontal;
                var min_plus = delta > 0 ? 1 : -1;
                frameCount = 0;

                if (min_plus * delta >= 0.25)
                {
                    if (min_plus * delta < 0.5)
                        graph.Change_coef(min_plus * 0.025);
                    else if (min_plus * delta < 0.75)
                        graph.Change_coef(min_plus * 0.05);
                    else
                        graph.Change_coef(min_plus * 0.075);
                }
            }

            if (graph.method == Method.Ellips || graph.method == Method.Giperbola)
            {
                var delta_x = joystick.Horizontal;
                var delta_y = joystick.Vertical;
                var minus_x = delta_x > 0 ? 1 : -1;
                var minus_y = delta_y > 0 ? 1 : -1;

                if (minus_x * delta_x >= 0.25)
                    if (minus_x * delta_x < 0.5) graph.Change_coef(minus_x * 0.1, 1);
                    else if (minus_x * delta_x < 0.75) graph.Change_coef(minus_x * 0.3, 1);
                    else graph.Change_coef(minus_x * 0.5, 1);

                if (minus_y * delta_y >= 0.25)
                    if (minus_y * delta_y < 0.5) graph.Change_coef(minus_y * 0.1, 2);
                    else if (minus_y * delta_y < 0.75) graph.Change_coef(minus_y * 0.3, 2);
                    else graph.Change_coef(minus_y * 0.5, 2);
            }
        }
        frameCount++;
    }
}
