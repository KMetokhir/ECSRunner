using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CameraInit : IEcsInitSystem {

        readonly EcsFilterInject<Inc<Player>> _filter = default;
        readonly EcsPoolInject<View> _viewPool = default;

        readonly EcsCustomInject<CameraSettings> _cameraSettings = default;

        public void Init (IEcsSystems systems) {          

            Camera mainCam = Camera.main;

            var world = systems.GetWorld();
            var entity = world.NewEntity();

            ref var cameraMain = ref world.GetPool<CameraMain>().Add(entity);
            cameraMain.Transform = mainCam.transform;

            ref var chaisComp = ref world.GetPool<ChasingComp>().Add(entity);
            chaisComp.FollowSpeed = _cameraSettings.Value.FollowSpeed;
            chaisComp.OffSetZ = _cameraSettings.Value.ZOffset;

            foreach (var filterEntity in _filter.Value)
            {
                ref View viewComp = ref _viewPool.Value.Get(filterEntity);
                chaisComp.Target = viewComp.Transform;

            }

        }
       
    }
}