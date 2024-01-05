using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class PlayerMovementUpgrade : Item
    {
        public bool _movement = false;
        public bool _jump = false;
        public bool _dashPower = false;
        public bool _dashCooldown = false;
 
        public override void itemUpgrade(GameObject player)
        {
            playSFX();

            if (_movement)
            {
                player.GetComponent<PlatformController>().upgradeMovementSpeed();
            }
            else if (_jump)
            {
                player.GetComponent<PlatformController>().upgradeJumpPower();
            }
            else if (_dashPower)
            {
                player.GetComponent<PlatformController>().upgradeDashPower();
            }
            else if (_dashCooldown)
            {
                player.GetComponent<PlatformController>().upgradeDashCooldown();
            }
        }

    }
}
