using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph_choosed_ctrl_ : MonoBehaviour
{
    private Vector2 btn_initial_pos;
    private Vector2 panel_initial_pos;

    private Vector2 joystick_initial_pos;
    private Vector2 btn_ctrl_initial_pos;

    private bool initial_pos_flag = true;

    // Start is called before the first frame update
    void Awake()
    {
        btn_initial_pos = transform.parent.Find("Apply_btn").GetComponent<RectTransform>().anchoredPosition;
        panel_initial_pos = transform.parent.Find("Method_panel").GetComponent<RectTransform>().anchoredPosition;

        joystick_initial_pos = transform.parent.Find("Fixed Joystick").GetComponent<RectTransform>().anchoredPosition;
        btn_ctrl_initial_pos = transform.parent.Find("Panel_graph_ctrl_btns").GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TrueOrFalse()
    {
        transform.GetComponent<Window_Graph_>().Change_choosed_bool(true);
        foreach (Transform child in transform.parent)
            if (child.name.Contains("window_graph_") && child.name != transform.name)
                child.GetComponent<Window_Graph_>().Change_choosed_bool(false);
        transform.parent.GetComponent<Graph_ctrl_with_btn_>().Set_current_graph(transform.name);
        transform.parent.Find("Fixed Joystick").GetComponent<Graph_ctrl_joystick_>().Set_current_graph(transform.name);

        if (initial_pos_flag)
        {
            transform.parent.Find("Apply_btn").GetComponent<RectTransform>().anchoredPosition = btn_initial_pos;
            transform.parent.Find("Method_panel").GetComponent<RectTransform>().anchoredPosition = panel_initial_pos;
            transform.parent.Find("Fixed Joystick").GetComponent<RectTransform>().anchoredPosition = joystick_initial_pos;
            transform.parent.Find("Panel_graph_ctrl_btns").GetComponent<RectTransform>().anchoredPosition = btn_ctrl_initial_pos;

            initial_pos_flag = !initial_pos_flag;
        }
    }
}
