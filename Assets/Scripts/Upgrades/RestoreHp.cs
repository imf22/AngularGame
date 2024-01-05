using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class RestoreHp : Item
    {
        public override void itemUpgrade(GameObject player)
        {
            playSFX();
            player.GetComponent<PlatformController>().healAmount(2);
            GameObject.Find("HpBarVisual").GetComponent<HPVisuals>().DrawHpBars();
        }
    }
}
