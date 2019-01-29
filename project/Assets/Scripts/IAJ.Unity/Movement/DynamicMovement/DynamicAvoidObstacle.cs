using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidObstacle : DynamicSeek
	{
		public Collider CollisionDetector;
		public float AvoidMargin;
		public float MaxLookAhead;
		public float WhiskerLength;
		public float WhiskerAngle;

		public override string Name
		{
			get { return "Avoid Obstacle"; }
		}

		public DynamicAvoidObstacle(GameObject obstacle)
		{
			this.Target = new KinematicData();
			this.CollisionDetector = obstacle.GetComponent<Collider>();
		}

		public override MovementOutput GetMovement()
		{
			Vector3 rayVector = Character.velocity.normalized;
			if (rayVector.magnitude == 0) rayVector = Util.MathHelper.ConvertOrientationToVector(Character.rotation);

            Debug.DrawRay(this.Character.Position, Util.MathHelper.Rotate2D(rayVector, -WhiskerAngle) * WhiskerLength, Color.black);
            Debug.DrawRay(this.Character.Position, Util.MathHelper.Rotate2D(rayVector, WhiskerAngle) * WhiskerLength, Color.black);
            Debug.DrawRay(this.Character.Position, rayVector * MaxLookAhead, Color.black);

			RaycastHit collision;
            if (CollisionDetector.Raycast(new Ray(this.Character.Position, Util.MathHelper.Rotate2D(rayVector, -WhiskerAngle)), out collision, WhiskerLength)) {
				base.Target.Position = this.Character.Position + Util.MathHelper.Rotate2D(rayVector, WhiskerAngle) * AvoidMargin;
				return base.GetMovement();
			}
			if (CollisionDetector.Raycast(new Ray(this.Character.Position, Util.MathHelper.Rotate2D(rayVector, WhiskerAngle)), out collision, WhiskerLength)) {
				base.Target.Position = this.Character.Position + Util.MathHelper.Rotate2D(rayVector,-WhiskerAngle) * AvoidMargin;
                return base.GetMovement();
			}
			if (CollisionDetector.Raycast(new Ray(this.Character.Position, rayVector), out collision, MaxLookAhead)) {
				base.Target.Position = collision.point + collision.normal * AvoidMargin;
				return base.GetMovement();
			}

			MovementOutput output = new MovementOutput();
            output.Clear();
			return output;

		}
	}
}
