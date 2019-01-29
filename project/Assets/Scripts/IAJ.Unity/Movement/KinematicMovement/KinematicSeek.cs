namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicSeek : KinematicMovement
    {
        public override string Name
        {
            get { return "Seek"; }
        }

        public KinematicSeek()
        {
            this.Output = new MovementOutput();
        }

        public override MovementOutput GetMovement()
        {
            this.Output.linear = this.Target.Position - this.Character.Position;

            if (this.Output.linear.sqrMagnitude > 0)
            {
                this.Output.linear.Normalize();
                this.Output.linear *= this.MaxSpeed;
            }

            return this.Output;
        }
    }
}
