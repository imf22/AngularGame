
using UnityEngine;

namespace angulargame
{
    public class FollowCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform target;
        public Vector3 offset;
        [Range(5,20)] public float camSmoothness;

        private void FixedUpdate()
        {
            if (target != null)
            {
                Follow();
            }
        }

        void Follow()
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, camSmoothness * Time.fixedDeltaTime);
            transform.position = smoothedPosition;
        }

        public void setTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }

}
