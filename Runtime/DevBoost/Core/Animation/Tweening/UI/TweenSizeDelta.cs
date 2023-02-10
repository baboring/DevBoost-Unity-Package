using UnityEngine;

namespace DevBoost.Mecani {
	/// <summary>
	/// This class will tween the size delta component of a give rect transform.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class TweenSizeDelta : UITweener {

		#region Data

		/// <summary>
		/// The transform to tween.
		/// </summary>
		[SerializeField]
		private RectTransform tweenTransform = null;

		/// <summary>
		/// The beginning size delta.
		/// </summary>
		[SerializeField]
		public Vector2 fromSizeDelta = Vector2.zero;

		/// <summary>
		/// The destination size delta of the 
		/// </summary>
		[SerializeField]
		public Vector2 toSizeDelta = Vector2.zero;

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// Grab the rect transform from the 
		/// </summary>
		protected override void Reset() {
			this.tweenTransform = this.GetComponent<RectTransform>();
			base.Reset();
		}

		#endregion

		#region UITweener

		/// <summary>
		/// Sets the starting size delta to the current value 
		/// </summary>
		public override void SetStartToCurrentValue() {
			this.fromSizeDelta = this.tweenTransform.sizeDelta;
		}

		/// <summary>
		/// Sets the end size delta to the current value.
		/// </summary>
		public override void SetEndToCurrentValue() {
			this.toSizeDelta = this.tweenTransform.sizeDelta;
		}

		/// <summary>
		/// Handle updating the tween.
		/// </summary>
		/// <param name="factor">The current tween factor to use as a lerp value for the vectors.</param>
		/// <param name="isFinished">Flag inidicating that the tween is finished.</param>
		protected override void TweenUpdate(float factor, bool isFinished) {
			this.tweenTransform.sizeDelta = Vector2.LerpUnclamped(fromSizeDelta, toSizeDelta, factor);
		}

		#endregion
	}
}
