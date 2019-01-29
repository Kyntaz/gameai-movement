using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.Arbitration
{
    public class PriorityMovement : DynamicMovement.DynamicMovement
    {

        public List<DynamicMovement.DynamicMovement> Movements { get; private set; }
        public DynamicMovement.DynamicMovement LastMovementPerformed { get; private set; }

        private MovementOutput ZeroMovement { get; set; }

        public override string Name
        {
            get
            {
                if (this.LastMovementPerformed == null) return "PriorityMovement";
                else return "PriorityMovement:" + this.LastMovementPerformed.Name;
            }
        }

        public override KinematicData Target
        {
            get
            {
                if (this.LastMovementPerformed == null) return null;
                else return this.LastMovementPerformed.Target;
            }
            set
            {
                //sets the base class property
                base.Target = value;
            }
        }

        public override Color DebugColor
        {
            get
            {
                if (this.LastMovementPerformed == null) return base.DebugColor;
                else return this.LastMovementPerformed.DebugColor;
            }

            set
            {
                //sets the base class property
                base.DebugColor = value;
            }
        }

        

        public PriorityMovement()
        {
            this.Movements = new List<DynamicMovement.DynamicMovement>();
            this.ZeroMovement = new MovementOutput();
        }

        public override MovementOutput GetMovement()
        {
            MovementOutput output;

            foreach (DynamicMovement.DynamicMovement movement in this.Movements)
            {
                movement.Character = this.Character;

                output = movement.GetMovement();

                if (output.SquareMagnitude() > 0)
                {
                    this.LastMovementPerformed = movement;

                    return output;
                }
            }

            //if we reach this point, no movement was performed, return empty output
            this.LastMovementPerformed = null;
            return this.ZeroMovement;
        }
    }
}
