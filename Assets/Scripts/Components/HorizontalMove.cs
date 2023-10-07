using UnityEngine;

namespace Client {

    struct HorizontalMove {
       
        public float MoveForce;
        public Vector3 Direction;

        public Vector3 Rotation;
        public float RotationSpeed;
        public float RotationSpeedToForwardDirection;

        public bool IsMoving;
    }
}