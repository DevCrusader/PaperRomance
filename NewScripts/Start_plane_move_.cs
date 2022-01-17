using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Start_plane_move_ : MonoBehaviour
{
    [SerializeField] private Main_window_graph_ Main_Window_Graph;
    [SerializeField] private string levelName;

    private bool start_movement = false;
    private List<Vector2> value_list;

    private float speed = 1;
    private Vector2 fromVector;
    private Vector2 toVector;
    private float progress;
    private int index_current_value = 1;
    private int frame_count = 1;

    // Start is called before the first frame update
    void Start()
    {
        fromVector = transform.GetComponent<RectTransform>().anchoredPosition;
        toVector = fromVector;

        transform.gameObject.GetComponent<Button>().interactable = false;
        transform.parent.Find("Finish_panel").gameObject.SetActive(false);
        transform.parent.Find("Collision_panel").gameObject.SetActive(false);
        //Debug.Log(transform.parent.Find("window-graph").gameObject.GetComponent(ScriptName));
    }

    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime * speed;
        if (start_movement)
        {
            transform.GetComponent<RectTransform>().anchoredPosition =
                Vector3.Lerp(fromVector, toVector, progress);

            if (frame_count < 10000000)
                frame_count += 1;

            toVector = value_list[index_current_value];
            if (frame_count % 3 == 0)
            {
                if (index_current_value < value_list.Count - 1)
                    index_current_value += 1;
                else
                {
                    if (GlobalV.dict_[levelName][0] != 0)
                    {
                        GlobalV.AddLevel(levelName);
                        GlobalV.AddLevel(levelName.Substring(0, levelName.Length - 1) +
                            (int.Parse(levelName[levelName.Length - 1].ToString()) + 1).ToString());
                        transform.parent.GetComponent<StartScript>().Start_();
                        transform.parent.Find("Finish_panel").gameObject.SetActive(true);
                    }
                    else
                        transform.parent.Find("Collision_panel").gameObject.SetActive(true);
                }
            }
        }
    }

    public void Start_move()
    {
        start_movement = true;
        value_list = Main_Window_Graph.Points_for_movement();
        //Debug.Log(String.Join(" ", new List<string> { "MAIN:", value_list.Count.ToString() }));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            //Debug.Log("COLLECT!");
            var a = transform.parent.Find("CurCount").GetComponent<Text>();
            a.text = (int.Parse(a.text) + 1).ToString();
            GlobalV.SetHeartCount(levelName, int.Parse(a.text));
            //Debug.Log(GlobalV.dict_[levelName][0]);

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Bird"))
        {
            Debug.Log("BIRD!");
            speed = 0;
            transform.parent.Find("Collision_panel").gameObject.SetActive(true);
        }
    }
}
