using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class ProjSpeedDownUpgrade : Item
    {
        //public AudioSource audioSource;
 
        public override void itemUpgrade(GameObject player)
        {
            playSFX();
            player.GetComponent<PlatformController>().upgradeProjSpeedDown();
        }
    }
}
