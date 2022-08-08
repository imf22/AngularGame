using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class testCubeMovement : MonoBehaviour
    {
        //public float moveSpeed = 10f;
        
        //public float jumpRate = 3f;
        //public float jumpPower = 15f;
        //float jumptimer;

        private void Start()
        {
            //jumptimer = 0f;
        }
        void Update()
        {
            //jumptimer -= Time.deltaTime;

            //if(VirtualInputManager.Instance.MoveLeft && VirtualInputManager.Instance.MoveRight)
            //{
            //    return;
            //}

            //if (VirtualInputManager.Instance.MoveRight)
            //{
            //    this.gameObject.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //    this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            //}

            //if (VirtualInputManager.Instance.MoveLeft)
            //{
            //    this.gameObject.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //    this.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            //}

            //if (VirtualInputManager.Instance.tempJump && jumptimer <= 0f)
            //{
            //    this.gameObject.transform.Translate(Vector3.up * (jumpPower / 10));
            //    jumptimer = jumpRate;
            //}

            if (VirtualInputManager.Instance.reset)
            {
                this.gameObject.transform.position = Vector3.up;
                
            }



        }
    }
}

