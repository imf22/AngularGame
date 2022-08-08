using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthVisuals : MonoBehaviour
{
    [SerializeField] private Sprite healthSprite4;
    [SerializeField] private Sprite healthSprite3;
    [SerializeField] private Sprite healthSprite2;
    [SerializeField] private Sprite healthSprite1;
    [SerializeField] private Sprite healthSprite0;

    private List<HealthContainer> healthContainerImageList;

    private void Awake()
    {
        healthContainerImageList = new List<HealthContainer>();
    }
    private void Start()
    {
        CreateHealthImage(new Vector2(-20, -200)).SetHealthFragment(4);
        CreateHealthImage(new Vector2(20, -200)).SetHealthFragment(2);
        CreateHealthImage(new Vector2(60, -200)).SetHealthFragment(3);
        CreateHealthImage(new Vector2(100, -200)).SetHealthFragment(0);
    }
    private HealthContainer CreateHealthImage(Vector2 anchoredPosition)
    {
        // Creat Image object
        GameObject healthContainer = new GameObject("Heart", typeof(Image));

        // Set as child of of object transform
        healthContainer.transform.parent = transform;
        healthContainer.transform.localPosition = Vector3.zero;

        // set Postition and size
        healthContainer.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        healthContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);

        // Set sprite Image
        Image healthContainerImgUI = healthContainer.GetComponent<Image>();
        healthContainerImgUI.sprite = healthSprite4;

        HealthContainer containerImg = new HealthContainer(this, healthContainerImgUI);
        healthContainerImageList.Add(containerImg);
        return containerImg;

    }
    
    // Represents 1 health container
    public class HealthContainer
    {
        private Image containerImg;
        private HealthVisuals healthVisuals;
        public HealthContainer(HealthVisuals healthVisuals, Image containerImg)
        {
            this.healthVisuals = healthVisuals;
            this.containerImg = containerImg;
        }

        public void SetHealthFragment(int framgent)
        {
            switch (framgent)
            {
                case 0: containerImg.sprite = healthVisuals.healthSprite0; break;
                case 1: containerImg.sprite = healthVisuals.healthSprite1; break;
                case 2: containerImg.sprite = healthVisuals.healthSprite2; break;
                case 3: containerImg.sprite = healthVisuals.healthSprite3; break;
                case 4: containerImg.sprite = healthVisuals.healthSprite4; break;
            }
        }
    }
}
