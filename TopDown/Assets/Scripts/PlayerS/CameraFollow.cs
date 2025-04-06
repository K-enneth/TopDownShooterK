using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset = new(0, 0, -10);
    private float _smoothTime = 0.25f;
    private Vector3 _velocity = Vector3.zero;
    
    [SerializeField] private Transform target;

    void Update()
    {
        var targetPos = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, _smoothTime);
    }
}
