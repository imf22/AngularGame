using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace angulargame
{
    public class KeyboardInput : MonoBehaviour
    {

        void Update()
        {
            // Move Right
            if (Input.GetKey(KeyCode.D))
            {
                VirtualInputManager.Instance.MoveRight = true;
            }
            else
            {
                VirtualInputManager.Instance.MoveRight = false;
            }
            //bool a = Input.GetKey(KeyCode.D);
            ////bool Input.GetKey(KeyCode.D) = true? VirtualInputManager.Instance.MoveRight = true : VirtualInputManager.Instance.MoveRight = false;
            //a = true ? VirtualInputManager.Instance.MoveRight = true : VirtualInputManager.Instance.MoveRight = false;



            // Move Left
            if (Input.GetKey(KeyCode.A))
            {
                VirtualInputManager.Instance.MoveLeft = true;
            }
            else
            {
                VirtualInputManager.Instance.MoveLeft = false;
            }

            // Drop
            if (Input.GetKey(KeyCode.S))
            {
                VirtualInputManager.Instance.Drop = true;
            }
            else
            {
                VirtualInputManager.Instance.Drop = false;
            }

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                VirtualInputManager.Instance.Jump = true;
            }
            else
            {
                VirtualInputManager.Instance.Jump = false;
            }

            // Shoot
            if (Input.GetMouseButton(0))
            {
                VirtualInputManager.Instance.Shoot = true;
            }
            else
            {
                VirtualInputManager.Instance.Shoot = false;
            }

            // Dash
            if (Input.GetMouseButtonDown(1))
            {
                VirtualInputManager.Instance.Dash = true;
            }
            else
            {
                VirtualInputManager.Instance.Dash = false;
            }

            // Reset Position reset
            if (Input.GetKey(KeyCode.R))
            {
                VirtualInputManager.Instance.reset = true;
            }
            else
            {
                VirtualInputManager.Instance.reset= false;
            }


        }
    }
}
