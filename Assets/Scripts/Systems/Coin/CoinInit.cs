using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CoinInit : IEcsInitSystem {

        readonly EcsCustomInject<CoinSettings> _coinSettings = default;

        public void Init (IEcsSystems systems) {          

            var gos = GameObject.FindGameObjectsWithTag("Coin");

            var world = systems.GetWorld();

            foreach (var go in gos)
            {
                var entity = world.NewEntity();         

                ref var coinComp = ref world.GetPool<Coin>().Add(entity);
                coinComp.Transform =go.transform;
                coinComp.DestoyParticles = go.GetComponentInChildren<ParticleSystem>();
                coinComp.Renderer = go.GetComponentInChildren<MeshRenderer>();
                coinComp.Collider = go.GetComponent<BoxCollider>();

                ref var rotationComp = ref world.GetPool<ClockwiseRotation>().Add(entity);
                rotationComp.RotationSpeed = _coinSettings.Value.RotationSpeed;
            }

        }
    }
}