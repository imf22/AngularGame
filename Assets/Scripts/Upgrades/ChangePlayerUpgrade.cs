using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class ChangePlayerUpgrade : Item
    {
        public bool _triangle = false;
        public bool _square = false;
        public bool _hex = false;
        public bool _oct = false;
 
        public override void itemUpgrade(GameObject player)
        {
            playSFX();

            if (_triangle)
            {
                player.GetComponent<PlatformController>().swapCharacter("Triangle");
            }
            if (_hex)
            {
                player.GetComponent<PlatformController>().swapCharacter("Hex");
            }
            if (_oct)
            {
                player.GetComponent<PlatformController>().swapCharacter("Oct");
            }
            
        }
    }
}
