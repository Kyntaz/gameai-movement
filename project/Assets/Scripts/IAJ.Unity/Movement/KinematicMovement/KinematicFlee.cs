namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicFlee : KinematicMovement
    {
        public override string Name
        {
            get { return "Flee"; }
        }

        public KinematicFlee()
        {
            this.Output = new MovementOutput();
        }

        public override MovementOutput GetMovement()
        {
            this.Output.linear = this.Character.Position - this.Target.Position;

            if (this.Output.linear.sqrMagnitude > 0)
            {
                this.Output.linear.Normalize();
                this.Output.linear *= this.MaxSpeed;
            }

            return this.Output;
        }
    }
}
