using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using UnityEngine.SceneManagement;

namespace Client {
    sealed class RestartSystem : IEcsRunSystem
    { 
        readonly EcsFilterInject<Inc<RestartEvent>> _filter = default;

        readonly EcsCustomInject<SceneData> _sceneData = default;

        public void Run (IEcsSystems systems) {

            foreach (var entity in _filter.Value)
            {

                _sceneData.Value.RestartButton.gameObject.SetActive(false);

                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);

            }
               
        }
    }
}