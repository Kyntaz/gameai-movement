using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicCharacter
    {
        private KinematicMovement movement;

        public KinematicMovement Movement
        {
            get { return this.movement; }
            set
            {
                this.movement = value;
                if(this.movement != null) this.movement.Character = this.StaticData;
            }
        }

        public StaticData StaticData { get; protected set; }

        public GameObject GameObject { get; protected set; }

        public KinematicCharacter(GameObject gameObject)
        {
            this.GameObject = gameObject;
            this.StaticData = new StaticData(gameObject.transform);
        }
        public void Update()
        {
            if (this.Movement != null) 
            {
                MovementOutput output = this.Movement.GetMovement();

                if (output != null)
                {
                    this.StaticData.Integrate(output, Time.deltaTime);

                    if (!(this.Movement is KinematicWander))
                    {
                        this.StaticData.SetOrientationFromVelocity(output.linear);
                    }
                }
            }
        }
    }
}
