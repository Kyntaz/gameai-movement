using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public abstract class KinematicMovement : Movement
    {
        protected MovementOutput Output { get; set; }
        public StaticData Character { get; set; }
        public float MaxSpeed { get; set; }
        public StaticData Target { get; set; }

        public KinematicMovement()
        {
            this.DebugColor = Color.black;
        }
    }
}
