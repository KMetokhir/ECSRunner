using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DestroySystem : IEcsRunSystem {

        readonly EcsFilterInject<Inc<DestroyEvent, Coin>> _filter = default;
   
        readonly EcsPoolInject<Coin> _coinPool = default;

        public void Run (IEcsSystems systems) {           

            foreach(var entity in _filter.Value)
            {
                ref Coin coinComp = ref _coinPool.Value.Get(entity);

                coinComp.DestoyParticles.Play();
                coinComp.Collider.enabled = false;
                coinComp.Renderer.enabled = false;
            }
        }
    }
}