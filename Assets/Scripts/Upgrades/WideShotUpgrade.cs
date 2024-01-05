using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class WideShotUpgrade : Item
    {

        public override void itemUpgrade(GameObject player)
        {
            playSFX();
            player.GetComponent<PlatformController>().upgradeWideShot();
        }
    }
}
