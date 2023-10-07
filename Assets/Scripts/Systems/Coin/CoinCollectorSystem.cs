using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using TMPro;

namespace Client {
    sealed class  CoinCollectorSystem : IEcsRunSystem {

        readonly EcsFilterInject<Inc<GetCoinEvent>> _filter = default;
        readonly EcsFilterInject<Inc<Coin>> _coinFilter = default;

        readonly EcsPoolInject<GetCoinEvent> _bonusEventPool = default;
        readonly EcsPoolInject<Coin> _coinPool = default;
        readonly EcsPoolInject<DestroyEvent> _destroyEventPool = default;


        readonly EcsCustomInject<SceneData> _sceneData = default;
        readonly EcsCustomInject<GameState> _gameState = default;


        public void Run (IEcsSystems systems) {         

            foreach (var entity in _filter.Value)
            {
                ref var bonusEventComp = ref _bonusEventPool.Value.Get(entity);

                foreach(var coinEnt in _coinFilter.Value)
                {
                    ref Coin coinComp = ref _coinPool.Value.Get(coinEnt);

                    if (bonusEventComp.Transform== coinComp.Transform)
                    {                     
                        _destroyEventPool.Value.Add(coinEnt);
                    }
                }               
               
                _gameState.Value.IncreaseCoinbalance(1);
              
                _sceneData.Value.CoinValuePlate.text = _gameState.Value.CoinBalance.ToString();

            }
        }
    }
}