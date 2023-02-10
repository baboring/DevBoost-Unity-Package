using UnityEngine;

namespace DevBoost.Mecani {

	public abstract class TweenVector : UITweener {

		#region Data

		[SerializeField, Header("----- Tween Vector -----")]
		private Vector3 from = Vector3.zero;
		/// <summary>
		/// Euler angles to tween from.
		/// </summary>
		public Vector3 From {
			get {
				return this.from;
			}
		}

		[SerializeField]
		private Vector3 to = Vector3.zero;
		/// <summary>
		/// Euler angles to tween to.
		/// </summary>
		public Vector3 To {
			get {
				return this.to;
			}
		}

		/// <summary>
		/// The vector to tween.
		/// </summary>
		protected abstract Vector3 Value {
			get; set;
		}
		#endregion

		#region UITweener

		/// <summary>
		/// Sets the starting value to the current value.
		/// </summary>
		public override void SetStartToCurrentValue() {
			this.from = this.Value;
		}

		/// <summary>
		/// Set the end to the current rotation of the transform.
		/// </summary>
		public override void SetEndToCurrentValue() {
			this.to = this.Value;
		}

		/// <summary>
		/// Just lerp the rotation.
		/// </summary>
		/// <param name="factor"></param>
		/// <param name="isFinished"></param>
		protected override void TweenUpdate(float factor, bool isFinished) {
			this.Value = Vector3.LerpUnclamped(this.from, this.to, factor);
		}
		
		/// <summary>
		/// Sets the from and to positions.
		/// Used when the from and to positions can't be set in the editor.
		/// </summary>
		/// <param name="startPosition">The start position of the tween.</param>
		/// <param name="endPosition">The end position of the tween.</param>
		public virtual void SetFromAndTo(Vector3 startPosition, Vector3 endPosition) {
			this.from = startPosition;
			this.to = endPosition;
		}

		#endregion
	}
}
