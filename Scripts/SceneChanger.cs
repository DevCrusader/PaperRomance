using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }


    public void StartGame()
    {
        GlobalV.InitializeDictionary();
        SceneManager.LoadScene("Main-menu");
    }

    public void ChangeActive(GameObject obj)
    {
        obj.gameObject.SetActive(!obj.gameObject.activeSelf);
        if(obj.gameObject.activeSelf)
            transform.parent.GetComponent<StartScript>().Start_();
    }
}
