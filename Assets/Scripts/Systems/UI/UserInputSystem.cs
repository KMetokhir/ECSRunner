using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.Scripting;

namespace Client
{
    sealed class UserInputSystem : EcsUguiCallbackSystem
    {
        private Camera mainCam = Camera.main;

        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc< Player, HorizontalMove, InputSwipeMagnitude>> _filter = default;

        readonly EcsPoolInject<HorizontalMove> _horizontalMovePool = default;  
        readonly EcsPoolInject<RestartEvent> _restartEventPool = default;
        readonly EcsPoolInject<InputSwipeMagnitude> _inputSwipePool = default;

        Vector3 _lastPointerPosition = default;

        bool _isPressed= false;

        #region "Restart Button"
        [Preserve]
        [EcsUguiClickEvent("Restart", "Events")]
        void OnRestartButtonClicked(in EcsUguiClickEvent e)
        {      
            _restartEventPool.Value.Add(_world.Value.NewEntity());         
          
        }
        #endregion

        #region "Swipe and mouse position"

        [Preserve]
        [EcsUguiDownEvent("TouchListener", "Events")]
        void OnDownTouchListener(in EcsUguiDownEvent e)
        {
            _lastPointerPosition = Input.mousePosition;
         
            _isPressed = true;
        }

        [Preserve]
        [EcsUguiUpEvent("TouchListener", "Events")]
        void OnUpTouchListener(in EcsUguiUpEvent e)
        {        
            _isPressed = false;           
            
                foreach (var entity in _filter.Value)
                {
                ref var horizontalMoveComp = ref _horizontalMovePool.Value.Get(entity);
                ref var swipeComp = ref _inputSwipePool.Value.Get(entity);

                horizontalMoveComp.IsMoving = false;
                horizontalMoveComp.Direction = Vector3.zero;
                horizontalMoveComp.Rotation = Vector3.forward;
                swipeComp.Magnitude = 0f;
            }              
            
        }

        public override void Run(IEcsSystems systems)
        {
            base.Run(systems);

            if (_isPressed)
            {
               var magnitude = mainCam.ScreenToViewportPoint(Input.mousePosition - _lastPointerPosition).magnitude;

                foreach (var entity in _filter.Value)
                {
                    ref var horizontalMoveComp = ref _horizontalMovePool.Value.Get(entity);
                    ref var inputSwipeComp = ref _inputSwipePool.Value.Get(entity);

                    inputSwipeComp.Magnitude = magnitude;

                    if (Input.mousePosition.x > _lastPointerPosition.x)
                    {
                        horizontalMoveComp.Direction = Vector3.right;
                        horizontalMoveComp.Rotation = Vector3.right + Vector3.forward;
                        horizontalMoveComp.IsMoving = true;                                              
                    }
                    else if (Input.mousePosition.x < _lastPointerPosition.x)
                    {
                        horizontalMoveComp.Direction = Vector3.left;
                        horizontalMoveComp.Rotation = Vector3.left + Vector3.forward;
                        horizontalMoveComp.IsMoving = true;
                    }                    
                }               

                _lastPointerPosition = Input.mousePosition;
            }

            #endregion
        }

    }
}