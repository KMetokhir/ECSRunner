using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class KeepInBoundsSystem : IEcsRunSystem {

        readonly EcsFilterInject<Inc< HorizontalMove, Player, View>> _filter = default;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<HorizontalMove> _horizontalMovePool = default;

        readonly EcsCustomInject<SceneData> _sceneData = default;

        public void Run(IEcsSystems systems)
        {

            foreach (var entity in _filter.Value)
            {               
                ref HorizontalMove horizontalMoveComp = ref _horizontalMovePool.Value.Get(entity);
                ref View viewComp = ref _viewPool.Value.Get(entity);

                if (viewComp.Transform.position.x > _sceneData.Value.XGroundBounds.y)
                {
                    KeepInBpounds(_sceneData.Value.XGroundBounds.y, viewComp.Rigidbody);
                    horizontalMoveComp.IsMoving = false;
                }
                else if (viewComp.Transform.position.x < _sceneData.Value.XGroundBounds.x)
                {
                    KeepInBpounds(_sceneData.Value.XGroundBounds.x, viewComp.Rigidbody);
                    horizontalMoveComp.IsMoving = false;
                }
            }
        }

        private void KeepInBpounds(float xBound, Rigidbody targetRB)
        {       
            targetRB.MovePosition(new Vector3(xBound, targetRB.position.y, targetRB.position.z));

            targetRB.velocity = new Vector3(0f, targetRB.velocity.y, targetRB.velocity.z);
        }
    }
}