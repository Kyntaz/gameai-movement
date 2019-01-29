using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicWander : KinematicMovement
    {
        public override string Name
        {
            get { return "Wander"; }
        }

        public float MaxRotation { get; set; }

        public KinematicWander()  
        {
            this.MaxRotation = 8*MathConstants.MATH_PI;
            this.Output = new MovementOutput();
        }

        public override MovementOutput GetMovement()
        {
            // Move forward in the current direction
            this.Output.linear = this.Character.GetOrientationAsVector();
            this.Output.linear *= this.MaxSpeed;

            // Turn a little
            this.Output.angular = RandomHelper.RandomBinomial() * this.MaxRotation;

            return this.Output;
        }
    }
}
