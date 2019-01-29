using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicCharacter 
    {
        public GameObject GameObject { get; protected set; }
        public KinematicData KinematicData { get; protected set; }
        private DynamicMovement movement;
        public DynamicMovement Movement 
        { 
            get { return this.movement; }
            set
            {
                this.movement = value;
                if(this.movement != null) this.movement.Character = this.KinematicData;
            } 
        }
        public float Drag { get; set; }
        public float MaxSpeed { get; set; }

        public DynamicCharacter(GameObject gameObject)
        {
            this.KinematicData = new KinematicData(new StaticData(gameObject.transform));
            this.GameObject = gameObject;
            this.Drag = 1;
            this.MaxSpeed = 20.0f;
        }
	
        // Update is called once per frame
        public void Update ()
        {
            if (this.Movement != null) 
            {
                MovementOutput output = this.Movement.GetMovement();

                if (output != null)
                {
                    Debug.DrawRay(this.GameObject.transform.position, output.linear,this.Movement.DebugColor);
                    
                    this.KinematicData.Integrate(output, this.Drag, Time.deltaTime);
                    this.KinematicData.SetOrientationFromVelocity();
                    this.KinematicData.TrimMaxSpeed(this.MaxSpeed);
                }
            }
        }
    }
}
