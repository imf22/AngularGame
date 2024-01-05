using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace angulargame
{
    public class VirtualInputManager : Singleton<VirtualInputManager>
    {
        public bool MoveRight;
        public bool MoveLeft;
        public bool Jump;
        public bool Drop;
        public bool Shoot;
        public bool Dash;

        public bool reset;
        public bool plus;
        public bool pause;

        public bool hardMode { get; internal set; }

        // TO DO
        // Pause Button
    }
}


