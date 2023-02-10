using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Mecani {

	/// <summary>
	/// This value tweens the alpha of the canvas group.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class TweenCanvasGroupAlpha : TweenFloat {

		#region Data

		/// <summary>
		/// The canvas group to tween.
		/// </summary>
		[SerializeField]
		private CanvasGroup canvasGroup = null;

		/// <summary>
		/// The value is the canvas group alpha value.
		/// </summary>
		/// <returns></returns>
		protected override float Value {
			get {
				return this.canvasGroup.alpha;
			}
			set {
				this.canvasGroup.alpha = value;
			}
		}

		#endregion

		#region Monobehaviour

		/// <summary>
		/// Get the CanvasGroup on reset.
		/// </summary>
		protected override void Reset() {
			this.canvasGroup = this.GetComponent<CanvasGroup>();
			base.Reset();
		}

		#endregion
	}
}
