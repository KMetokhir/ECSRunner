
using UnityEngine;

namespace Client {

    struct Coin {

        public Transform Transform;
        public MeshRenderer Renderer;
        public BoxCollider Collider;
        public ParticleSystem DestoyParticles;
    }
}