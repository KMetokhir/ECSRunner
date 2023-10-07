using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CoinClockwiseRotationSystem : AbstractClockwiseRotationSystem, IEcsRunSystem 
    {

        readonly EcsFilterInject<Inc<Coin,ClockwiseRotation>> _filter = default;

        readonly EcsPoolInject<Coin> _coinPool = default;
        readonly EcsPoolInject<ClockwiseRotation> _constantRotationPool = default;

        public void Run(IEcsSystems systems)
        {

            foreach (var entity in _filter.Value)
            {
                ref Coin coinComp = ref _coinPool.Value.Get(entity);
                ref ClockwiseRotation rotComp = ref _constantRotationPool.Value.Get(entity);
           
                Rotate(coinComp.Transform, rotComp.RotationSpeed, Time.deltaTime);

            }
        }
    }
}