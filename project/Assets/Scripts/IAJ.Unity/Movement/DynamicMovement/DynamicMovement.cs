using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public abstract class DynamicMovement : Movement
    {
        protected MovementOutput Output { get; set; }

        public KinematicData Character { get; set; }
        virtual public KinematicData Target { get; set; }

        public float MaxAcceleration { get; set; }

        public DynamicMovement()
        {
            this.DebugColor = Color.black;
        }
    }
}
