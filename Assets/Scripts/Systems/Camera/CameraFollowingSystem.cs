using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CameraFollowingSystem : IEcsRunSystem {

        readonly EcsFilterInject<Inc<CameraMain, ChasingComp>> _filter = default;


        readonly EcsPoolInject<CameraMain> _cameraPool = default;
        readonly EcsPoolInject<ChasingComp> _chasingPool = default;
       
        public void Run (IEcsSystems systems) {

            foreach (var entity in _filter.Value)
            {
               ref CameraMain cameraComp = ref _cameraPool.Value.Get(entity);
               ref ChasingComp chaseComp = ref _chasingPool.Value.Get(entity);

               cameraComp.Transform.position = Vector3.Lerp(cameraComp.Transform.position, GetTargetPosition(cameraComp.Transform, chaseComp.Target, chaseComp.OffSetZ), chaseComp.FollowSpeed * Time.fixedDeltaTime);               

            }
           
        }
        

        private Vector3 GetTargetPosition(Transform cam, Transform target, float zOffSet)
        {
            return new Vector3(target.position.x, cam.position.y, target.position.z - zOffSet);
        }
    }
}