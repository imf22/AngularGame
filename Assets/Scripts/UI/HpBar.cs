using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Sprite full, empty;
    private Image hpBarImage;

    private void Awake()
    {
        hpBarImage = GetComponent<Image>();
    }

    public void SetHpBarImage(hpBarStatus status)
    {
        switch (status)
        {
            case hpBarStatus.Empty:
                hpBarImage.sprite = empty;
                break;
            case hpBarStatus.Full:
                hpBarImage.sprite = full;
                break;
        }
    }
}
public enum hpBarStatus
{
    Empty = 0,
    Full = 1
}

