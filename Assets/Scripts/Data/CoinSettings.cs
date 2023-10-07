using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Coin Settings", menuName = "Settings/Coin", order = 52)]

public class CoinSettings : ScriptableObject
{
    [SerializeField] private float _rotationSpeed;

    public float RotationSpeed => _rotationSpeed;

}
