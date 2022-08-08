
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        transform.position = target.position + offset;
    }
}
