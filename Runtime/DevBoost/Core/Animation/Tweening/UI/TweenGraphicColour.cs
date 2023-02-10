using UnityEngine;
using UnityEngine.UI;

namespace DevBoost.Mecani {

	/// <summary>
	/// This tweens the colour of a graphic.
	/// </summary>
	[RequireComponent(typeof(Graphic))]
	public class TweenGraphicColour : TweenColour {

		#region Data

		/// <summary>
		/// The graphic whose colour we are tweening.
		/// </summary>
		[SerializeField]
		private Graphic tweenGraphic = null;

		/// <summary>
		/// The current colour of the tweening graphic.
		/// </summary>
		/// <returns></returns>
		protected override Color ColourValue {
			get {
				return this.tweenGraphic.color;
			}
			set {
				this.tweenGraphic.color = value;
			}
		}

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// Get the tween graphic component.
		/// </summary>
		protected override void Reset() {
			this.tweenGraphic = this.GetComponent<Graphic>();
			base.Reset();
		}

		#endregion

	}

}
