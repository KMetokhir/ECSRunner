using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneData : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private  float _offSet;
    public Vector2 XGroundBounds
    {
        get
        {
            return new Vector2((_ground.position.x - _ground.localScale.x * 10f) / 2f + _offSet, (_ground.position.x + _ground.localScale.x * 10f) / 2f - _offSet);
        }
    }

    [SerializeField] private   TMP_Text _coinValuePlate;
     public TMP_Text CoinValuePlate => _coinValuePlate;

    [SerializeField] private Button _restartButton;
    public Button RestartButton=> _restartButton; 

}
