using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float moveSmoothness;
    [SerializeField] private float rotSmoothness;

    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private Vector3 rotOffset;

    [SerializeField] private Transform carTarget;

    private void FixedUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 targetPos = carTarget.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector3 direction = carTarget.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSmoothness * Time.deltaTime);
    }
}
