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
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.J))
            {
                VirtualInputManager.Instance.Shoot = true;
            }
            else
            {
                VirtualInputManager.Instance.Shoot = false;
            }

            // Dash
            if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.K))
            {
                VirtualInputManager.Instance.Dash = true;
            }
            else
            {
                VirtualInputManager.Instance.Dash = false;
            }

            // Pause
            if (Input.GetKey(KeyCode.P) )
            {
                VirtualInputManager.Instance.pause = true;
            }
            else
            {
                VirtualInputManager.Instance.pause = false;
            }


            // DEBUG KEYS

            // Reset Position reset
            if (Input.GetKey(KeyCode.Alpha1))
            {
                VirtualInputManager.Instance.reset = true;
            }
            else
            {
                VirtualInputManager.Instance.reset= false;
            }

            // Debug increase wave
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                VirtualInputManager.Instance.plus = true;
            }
            else
            {
                VirtualInputManager.Instance.plus = false;
            }

            // Debug increase wave
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                VirtualInputManager.Instance.hardMode = true;
            }
            else
            {
                VirtualInputManager.Instance.hardMode = false;
            }

        }
    }
}
