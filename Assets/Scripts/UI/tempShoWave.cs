using angulargame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempShoWave : MonoBehaviour
{
    public GameObject SpawnController;
    public Text textDisplay;
    private Coroutine _textUpdate;

    // Start is called before the first frame update
    void Start()
    {
        textDisplay = this.GetComponent<Text>();
        //textDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 200);
        SpawnController = GameObject.Find("SpawnerController");
    }

    // Update is called once per frame
    void Update()
    {
        if (_textUpdate != null)
        {
            StopCoroutine(_textUpdate);
        }
        _textUpdate = StartCoroutine(updateWave());
    }

    private IEnumerator updateWave()
    {
        textDisplay.text = "Wave: "+ SpawnController.GetComponent<SpawnController>().getCurrentWave();
        yield return new WaitForSeconds(0.1f);

    }
}
