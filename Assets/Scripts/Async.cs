using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Async : MonoBehaviour
{
    private AsyncOperation operation;
    public TextMeshProUGUI LoadText;


    private float precent = 10;
    private float widthOfPrecessing;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadSceneAndData());
    }

    IEnumerator loadSceneAndData()
    {
        operation = SceneManager.LoadSceneAsync("SampleScene");
        yield return operation;
    }

    // Update is called once per frame
    void Update()
    {
        precent = operation == null ? 10 : operation.progress * 100;
        Debug.Log(precent);
        LoadText.text = "Loading... " + precent + "%";

    }
}
