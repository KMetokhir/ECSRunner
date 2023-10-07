using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Settings", menuName = "Settings/Camera", order = 53)]

public class CameraSettings : ScriptableObject
{
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _zOffset;

    public float FollowSpeed =>_followSpeed;
    public float ZOffset =>_zOffset;
}
