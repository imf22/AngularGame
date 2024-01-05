using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class FirerateUpgrade : Item
    {
        //public AudioSource audioSource;
 
        public override void itemUpgrade(GameObject player)
        {
            playSFX();
            player.GetComponent<PlatformController>().upgradeFireRate();
        }
    }
}
