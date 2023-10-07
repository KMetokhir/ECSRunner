using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;


namespace Client {
    sealed class PlayerInit : IEcsInitSystem {

        readonly EcsCustomInject<PlayerSettings> _playerSettings = default;

        public void Init (IEcsSystems systems) {

            var go = GameObject.FindGameObjectWithTag("Player");

            var world = systems.GetWorld();
            var entity = world.NewEntity();

            world.GetPool<Player>().Add(entity);

            ref var inputSwipeComp = ref world.GetPool<InputSwipeMagnitude>().Add(entity);
            inputSwipeComp.Magnitude = 0f;

            ref var forwardMoveComp = ref world.GetPool<ForwardMove>().Add(entity);
            forwardMoveComp.Force = _playerSettings.Value.ZForce;
            forwardMoveComp.Acceleration = _playerSettings.Value.ZAcceleration;
            forwardMoveComp.MaxSpeed = _playerSettings.Value.ZMaxSpeed;

            ref var horizontalMoveComp = ref world.GetPool<HorizontalMove>().Add(entity);
            horizontalMoveComp.MoveForce = _playerSettings.Value.Xforce;
            horizontalMoveComp.Direction = Vector3.zero;
            horizontalMoveComp.RotationSpeed = _playerSettings.Value.RotationSpeed;
            horizontalMoveComp.RotationSpeedToForwardDirection = _playerSettings.Value.RotationSpeedToForwardDirection;

            ref var viewComp = ref world.GetPool<View>().Add(entity);
            viewComp.Transform = go.transform;
            viewComp.Rigidbody = go.GetComponent<Rigidbody>();
            viewComp.Animator = go.GetComponentInChildren<Animator>();
            viewComp.Animator.SetBool("isRunning", true);

            var mb = go.GetComponent<PlayerMonob>();
            mb.SetWorld(world);           
        }
    }
}