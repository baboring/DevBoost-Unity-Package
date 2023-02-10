using UnityEngine;

namespace DevBoost.Mecani {

	[RequireComponent(typeof(SpriteRenderer))]
	public class TweenSpriteColour : TweenColour {

		#region Data

		/// <summary>
		/// The graphic whose colour we are tweening.
		/// </summary>
		[SerializeField]
		private SpriteRenderer tweenRenderer = null;

		/// <summary>
		/// The current colour of the tweening graphic.
		/// </summary>
		/// <returns></returns>
		protected override Color ColourValue {
			get {
				return this.tweenRenderer.color;
			}
			set {
				this.tweenRenderer.color = value;
			}
		}

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// Get the tween graphic component.
		/// </summary>
		protected override void Reset() {
			this.tweenRenderer = this.GetComponent<SpriteRenderer>();
			base.Reset();
		}

		#endregion

	}

}
