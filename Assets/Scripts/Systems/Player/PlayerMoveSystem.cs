using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using UnityEngine;

namespace Client {
    sealed class PlayerMoveSystem : IEcsRunSystem 
    {  
        readonly EcsFilterInject<Inc<ForwardMove, HorizontalMove, Player, InputSwipeMagnitude, View>> _filter = default;
             
        readonly EcsPoolInject<ForwardMove> _forwardMovePool = default;
        readonly EcsPoolInject<HorizontalMove> _horizontalMovePool = default;
        readonly EcsPoolInject<InputSwipeMagnitude>_inputSwipePool = default;
        readonly EcsPoolInject<View> _viewPool = default;

        readonly EcsCustomInject<PlayerSettings> _playerSettings = default;

        float _forwardForce = 0f;         
       

        public void Run (IEcsSystems systems) 
        {

            foreach (var entity in _filter.Value)
            {
                ref ForwardMove forwardMoveComp = ref _forwardMovePool.Value.Get(entity);
                ref HorizontalMove horizontalMoveComp = ref _horizontalMovePool.Value.Get(entity);
                ref View viewComp = ref _viewPool.Value.Get(entity);
                ref InputSwipeMagnitude swiprComp = ref _inputSwipePool.Value.Get(entity);


                var magnitude = swiprComp.Magnitude * 1000f;
                magnitude = Mathf.Clamp(magnitude, 0f, 300f);


                #region "ForwardMove"             

                viewComp.Rigidbody.AddForce(Vector3.forward * _forwardForce , ForceMode.Acceleration);

                _forwardForce = Mathf.Lerp(_forwardForce, forwardMoveComp.Force, Time.fixedDeltaTime * forwardMoveComp.Acceleration);

                if (Mathf.Abs(viewComp.Rigidbody.velocity.z) > forwardMoveComp.MaxSpeed)
                {
                    viewComp.Rigidbody.velocity = new Vector3(viewComp.Rigidbody.velocity.x, viewComp.Rigidbody.velocity.y, forwardMoveComp.MaxSpeed);                  

                }

                #endregion


                #region "HorizontalMove"
                                 
                    if (horizontalMoveComp.IsMoving)
                    {
                        if (magnitude > 1f)
                        {

                            viewComp.Rigidbody.MoveRotation(Quaternion.Lerp(viewComp.Rigidbody.rotation, Quaternion.LookRotation(horizontalMoveComp.Rotation, Vector3.up),
                                Time.fixedDeltaTime * _playerSettings.Value.GetSwipeMagnitude(magnitude) * horizontalMoveComp.RotationSpeed));

                            viewComp.Rigidbody.AddForce(horizontalMoveComp.Direction* _playerSettings.Value.GetSwipeMagnitude(magnitude) * horizontalMoveComp.MoveForce, ForceMode.Force);                        
                                                    
                        }
                        else
                        {
                            RotateToForwardDirection(viewComp.Rigidbody, horizontalMoveComp.RotationSpeedToForwardDirection, Time.fixedDeltaTime);                                                 
                        }

                    }
                    else
                    {
                        RotateToForwardDirection(viewComp.Rigidbody, horizontalMoveComp.RotationSpeedToForwardDirection, Time.fixedDeltaTime);  
                    }               

                #endregion               
            }
        }

        private void RotateToForwardDirection(Rigidbody rB,float rotationSpeed, float time )
        {
            rB.MoveRotation(Quaternion.Lerp(rB.rotation, Quaternion.LookRotation(Vector3.forward, Vector3.up),
               time * rotationSpeed));
        }
       
    }
}

