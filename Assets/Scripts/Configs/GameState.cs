using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Client
{
    public class GameState
    {
        public int CoinBalance { get; private set; }
        
        public void IncreaseCoinbalance(int value)
        {
            value = Mathf.Clamp(value, 0, int.MaxValue);

            CoinBalance += value;
        }

    }
}
