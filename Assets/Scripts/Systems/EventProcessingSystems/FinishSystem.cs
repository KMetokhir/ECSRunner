using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class FinishSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<ForwardMove, HorizontalMove, Player, View>> _playerfilter = default;
        readonly EcsFilterInject<Inc<FinishEvent>> _eventfilter = default;

        readonly EcsPoolInject<ForwardMove> _forwardMovePool = default;
        readonly EcsPoolInject<HorizontalMove> _horizontalMovePool = default;
        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<ClockwiseRotation> _clockwiseRotPoll = default;

        readonly EcsCustomInject<PlayerSettings> _playerSettings = default;
        readonly EcsCustomInject<SceneData> _sceneData = default;

        public void Run (IEcsSystems systems) 
        {

            foreach (var eventEnt in _eventfilter.Value)
            {

                foreach (var entity in _playerfilter.Value)
                {
                    ref View viewComp = ref _viewPool.Value.Get(entity);

                    _forwardMovePool.Value.Del(entity);
                    _horizontalMovePool.Value.Del(entity);

                    ref ClockwiseRotation clockRotComp = ref  _clockwiseRotPoll.Value.Add(entity);
                    clockRotComp.RotationSpeed = _playerSettings.Value.ClockRotationSpeed;

                    viewComp.Rigidbody.drag = 100f;
                    viewComp.Animator.SetBool("isRunning", false);
                    viewComp.Animator.SetBool("isWaving", true);

                    _sceneData.Value.RestartButton.gameObject.SetActive(true);

                }
            }
        }
    }
}