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
                // register your systems here, for example:
                .Add(new PlayerInit())
                .Add(new CoinInit())
                //.Add(new PlayerMoveSystem())
                .Add(new CoinCollectorSystem())
                // .Add(new UserSwipeInputSystem())
                .Add(new CameraInit())
                .Add(new DestroySystem())
                .Add(new FinishSystem())
                .Add(new CoinClockwiseRotationSystem())
                .Add(new PlayerClockwiseRotationSystem())
                // .Add(new CameraFollowingSystem())
                //.AddWorld(new EcsWorld(), "Events")




                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
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
                // .InjectUgui(_uguiEmitter, "Events")
                .Init ();
            
            
            _fixedUpdateSystems = new EcsSystems(_world);

            _fixedUpdateSystems
                .Add(new PlayerMoveSystem())
                .Add(new CameraFollowingSystem())
                .Add(new UserInputSystem())
                .Add(new RestartSystem())
                .AddWorld(new EcsWorld(), "Events")
                .DelHere<RestartEvent>()
                .Inject(_sceneData)  
                .Inject(_playerSettings)
                .InjectUgui(_uguiEmitter, "Events")
                .Init();
                   

        }
         
        void Update () {
            // process systems here.
            _systems?.Run ();
        }

        void FixedUpdate()
        {
            _fixedUpdateSystems?.Run(); 
        }

        void OnDestroy () {
            if (_systems != null) {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy();
                _systems = null;
            }

            if (_fixedUpdateSystems != null)
            {
                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;
            }
            
            // cleanup custom worlds here.
            
            // cleanup default world.
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }
    }
}