using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace angulargame
{
    public class HPVisuals : MonoBehaviour
    {
        public GameObject hpBarPrefab;
        private GameObject Player;
        List<HpBar> hpBars = new List<HpBar>();
        private Coroutine _startHpDrawing;


        private void OnEnable()
        {
            PlatformController.OnPlayerDamaged += DrawHpBars;
        }
        private void OnDisable()
        {
            PlatformController.OnPlayerDamaged -= DrawHpBars;
        }

        public void Start()
        {
            StartCoroutine(startDrawingHp());
        }
        public void CreateEmptyHealth()
        {
            GameObject newhpBar = Instantiate(hpBarPrefab);
            newhpBar.transform.SetParent(transform);

            HpBar hpBarComponent = newhpBar.GetComponent<HpBar>();
            hpBarComponent.SetHpBarImage(hpBarStatus.Empty);
            hpBars.Add(hpBarComponent);
        }

        public void DrawHpBars()
        {
            ClearHpBars();

            if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
            {
                this.Player = GameObject.FindGameObjectsWithTag("Player")[0];
            }
            int hpBarsToMake = Player.GetComponent<PlatformController>().getMaxPlayerHealth();
            for (int i = 0; i < hpBarsToMake; i++)
            {
                CreateEmptyHealth();

            }

            for (int i = 0; i < hpBars.Count; i++)
            {
                int hpStatusRemainder = (int) Mathf.Clamp(Player.GetComponent<PlatformController>().getPlayerHealth() - i , 0, 1);
                hpBars[i].SetHpBarImage((hpBarStatus) hpStatusRemainder);
            }

        }

        public void ClearHpBars()
        {
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            hpBars = new List<HpBar>();
        }

        public void setPlayer(GameObject player)
        {
            this.Player = player;
        }

        public IEnumerator startDrawingHp()
        {
            //yield return new WaitForSeconds(0.001f);
            yield return new WaitForEndOfFrame();
            DrawHpBars();
            yield return null;
        }
    }
}

