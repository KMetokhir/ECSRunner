using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class PlayerClockwiseRotationSystem : AbstractClockwiseRotationSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Player, ClockwiseRotation, View>> _filter = default;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<ClockwiseRotation> _constantRotationPool = default;

        public void Run(IEcsSystems systems)        {

            foreach (var entity in _filter.Value)
            {
                ref View playerViewComp = ref _viewPool.Value.Get(entity);
                ref ClockwiseRotation rotComp = ref _constantRotationPool.Value.Get(entity);
               
                Rotate(playerViewComp.Transform, rotComp.RotationSpeed, Time.deltaTime);
            }
        }
    }
}