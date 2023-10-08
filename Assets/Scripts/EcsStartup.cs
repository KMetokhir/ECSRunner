using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;


namespace Client {
    sealed class EcsStartup : MonoBehaviour {

        [SerializeField] private  EcsUguiEmitter _uguiEmitter;

        [SerializeField] private  SceneData _sceneData;

        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private CoinSettings _coinSettings;
        [SerializeField] private CameraSettings _cameraSettings;

        EcsWorld _world;      
        
        IEcsSystems _systems;
        IEcsSystems _fixedUpdateSystems;

        void Start () {
            _world = new EcsWorld ();           
            GameState state = new GameState();

            _systems = new EcsSystems (_world);

            _systems                
                .Add(new PlayerInit())
                .Add(new CoinInit())                
                .Add(new CoinCollectorSystem())                
                .Add(new CameraInit())
                .Add(new DestroySystem())
                .Add(new FinishSystem())
                .Add(new CoinClockwiseRotationSystem())
                .Add(new PlayerClockwiseRotationSystem())
                
#if UNITY_EDITOR                
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .DelHere<GetCoinEvent>()
                .DelHere<DestroyEvent>()
                .DelHere<FinishEvent>()
                .Inject(state)
                .Inject(_sceneData)
                .Inject(_playerSettings)
                .Inject(_coinSettings)
                .Inject(_cameraSettings)               
                .Init ();
            
            
            _fixedUpdateSystems = new EcsSystems(_world);

            _fixedUpdateSystems               
                .Add(new CameraFollowingSystem())
                .Add(new UserInputSystem())
                .Add(new KeepInBoundsSystem())
                .Add(new PlayerMoveSystem())
                .Add(new RestartSystem())
                .AddWorld(new EcsWorld(), "Events")
                .DelHere<RestartEvent>()
                .Inject(_sceneData)  
                .Inject(_playerSettings)
                .InjectUgui(_uguiEmitter, "Events")
                .Init();                  

        }
         
        void Update () 
        {            
            _systems?.Run ();
        }

        void FixedUpdate()
        {
            _fixedUpdateSystems?.Run(); 
        }

        void OnDestroy () {
            if (_systems != null) 
            {                
                _systems.Destroy();
                _systems = null;
            }

            if (_fixedUpdateSystems != null)
            {
                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;
            }
            
            
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }

        }
    }
}