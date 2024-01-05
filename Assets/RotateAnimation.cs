using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public class RotateAnimation : MonoBehaviour

    {
        public Vector3 AxisOfRotation;
        public float AngleOfRotation;
        public Vector3 _targetRot;

        private Rigidbody _rigidbody;


        // Start is called before the first frame update
        void Start()
        {
            // For Normal movement
            _rigidbody = GetComponent<Rigidbody>();
        }


        void FixedUpdate()
        {
            Vector3 offsetRot = AxisOfRotation * AngleOfRotation ;
            _targetRot = _rigidbody.rotation.eulerAngles + offsetRot;
            Quaternion targetRotQuat = Quaternion.Euler(_targetRot);

            _rigidbody.MoveRotation(targetRotQuat);
        }
    }






}
