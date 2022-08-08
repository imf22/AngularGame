using angulargame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempShowPlayerHealth : MonoBehaviour
{
    public GameObject player;
    public Text textDisplay;
    private Coroutine _textUpdate;

    // Start is called before the first frame update
    void Start()
    {
        textDisplay = this.GetComponent<Text>();
        textDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        //player = GameObject.Find("PlayerCube");
        //_textUpdate = StartCoroutine(updateHealth());
    }


    private IEnumerator updateHealth()
    {
        while (player != null)
        {
            textDisplay.text = "HP: "+ player.GetComponent<PlatformController>().getPlayerHealth();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void startHpUI()
    {
        _textUpdate = StartCoroutine(updateHealth());
    }

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
}
