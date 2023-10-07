using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using UnityEngine;

namespace Client {
    sealed class PlayerMoveSystem : IEcsRunSystem 
    {      

        readonly EcsFilterInject<Inc<ForwardMove, HorizontalMove, Player, InputSwipeMagnitude>> _filter = default;
             
        readonly EcsPoolInject<ForwardMove> _forwardMovePool = default;
        readonly EcsPoolInject<HorizontalMove> _horizontalMovePool = default;
        readonly EcsPoolInject<InputSwipeMagnitude>_inputSwipePool = default;
        readonly EcsPoolInject<View> _viewPool = default;

        readonly EcsCustomInject<SceneData> _sceneData = default;
        readonly EcsCustomInject<PlayerSettings> _playerSettings = default;

        float _forwardForce = 0f;  

        private float _xVelocity = 0f;
        private float _xStopAcceleration = 10f;
       

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


                #region "HorizontalMove in Bounds"

                if (viewComp.Transform.position.x <= _sceneData.Value.XGroundBounds.y && viewComp.Transform.position.x >= _sceneData.Value.XGroundBounds.x)
                {                   
                    if (horizontalMoveComp.IsMoving)
                    {
                        if (magnitude > 1f)
                        {

                            viewComp.Rigidbody.MoveRotation(Quaternion.Lerp(viewComp.Rigidbody.rotation, Quaternion.LookRotation(horizontalMoveComp.Rotation, Vector3.up),
                                Time.fixedDeltaTime * _playerSettings.Value.GetSwipeMagnitude(magnitude) * horizontalMoveComp.RotationSpeed));

                            viewComp.Rigidbody.AddForce(horizontalMoveComp.Direction* _playerSettings.Value.GetSwipeMagnitude(magnitude) * horizontalMoveComp.MoveForce, ForceMode.Force);                        

                            _xVelocity = viewComp.Rigidbody.velocity.x;
                        }
                        else
                        {
                            StopHorizontalMove(viewComp.Rigidbody, _xStopAcceleration,  horizontalMoveComp.RotationSpeedToForwardDirection, ref _xVelocity);                           
                        }

                    }
                    else
                    {
                        StopHorizontalMove(viewComp.Rigidbody, _xStopAcceleration, horizontalMoveComp.RotationSpeedToForwardDirection, ref _xVelocity);      

                    }
                }
                else if(viewComp.Transform.position.x > _sceneData.Value.XGroundBounds.y)
                {                    
                    KeepInBpounds(_sceneData.Value.XGroundBounds.y, viewComp.Rigidbody, horizontalMoveComp.RotationSpeedToForwardDirection, ref _xVelocity);
                }
                else if (viewComp.Transform.position.x < _sceneData.Value.XGroundBounds.x)
                {                    
                    KeepInBpounds(_sceneData.Value.XGroundBounds.x, viewComp.Rigidbody, horizontalMoveComp.RotationSpeedToForwardDirection, ref _xVelocity);
                }

                #endregion               
            }
        }

        private void StopHorizontalMove(Rigidbody rB, float xAcceleration, float rotationSpeed,ref float xStopSpeed)
        {
            xStopSpeed = Mathf.Lerp(_xVelocity, 0f, Time.fixedDeltaTime * xAcceleration);

            rB.velocity = new Vector3(_xVelocity, rB.velocity.y, rB.velocity.z);

            rB.MoveRotation(Quaternion.Lerp(rB.rotation, Quaternion.LookRotation(Vector3.forward, Vector3.up),
                Time.fixedDeltaTime * rotationSpeed));
        }


        private void KeepInBpounds(float xBound, Rigidbody targetRB, float rotationSpeed, ref float xStopSpeed)
        {

            StopHorizontalMove(targetRB,100f, rotationSpeed, ref xStopSpeed);         

            targetRB.MovePosition(new Vector3(xBound, targetRB.position.y, targetRB.position.z));
            
        }
    }
}

