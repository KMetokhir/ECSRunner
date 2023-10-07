using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "Settings/Player", order = 51)]

 public class PlayerSettings : ScriptableObject
    {

    [Header("Forward Moving")]
    [SerializeField] private float _zForce;
    [SerializeField] private float _zAcceleration;
    [SerializeField] private float _zMaxSpeed;

    [Header("Horizontal Moving")]
    [SerializeField] private float _xforce;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotationSpeedToForwardDirection;
    [SerializeField] public AnimationCurve _swipeMagnitudeCorrectionCurve;

    [Header("ClockRotation")]
    [SerializeField] private float _clockRotationSpeed;

    public float ZForce=> _zForce;
    public float ZAcceleration=> _zAcceleration;
    public float ZMaxSpeed=> _zMaxSpeed;
    public float Xforce=> _xforce;
    public float RotationSpeed =>_rotationSpeed;
    public float RotationSpeedToForwardDirection =>_rotationSpeedToForwardDirection;
    public float ClockRotationSpeed=>_clockRotationSpeed;

    public float GetSwipeMagnitude(float inputSwipeMagnitude)
    {
        return _swipeMagnitudeCorrectionCurve.Evaluate(inputSwipeMagnitude);
    } 
}
