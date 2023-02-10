using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Mecani {

	/// <summary>
	/// This is a general class for tweening a specific colour from a value to a value 
	/// </summary>
	public abstract class TweenColour : UITweener {

		#region Data

		/// <summary>
		/// The colour to tween from.
		/// </summary>
		[SerializeField, Header("----- Tween Colour -----")]
		private Color fromColour = Color.white;

		/// <summary>
		/// The colour to tween to.
		/// </summary>
		[SerializeField]
		private Color toColour = Color.white;

		/// <summary>
		/// Access the colour component that needs to get tweened here.
		/// </summary>
		/// <returns></returns>
		protected abstract Color ColourValue {
			get; set;
		}

		#endregion

		#region 

		/// <summary>
		/// Based on the tween factor we should just lerp between the two transforms.
		/// </summary>
		/// <param name="factor">Tweening factor to use.</param>
		/// <param name="isFinished">Flag indicating the tween is finished.</param>
		protected override void TweenUpdate(float factor, bool isFinished) {
			this.ColourValue = Color.LerpUnclamped(this.fromColour, this.toColour, factor);
		}

		/// <summary>
		/// The the from colour to the current value.
		/// </summary>
		public override void SetStartToCurrentValue() {
			this.fromColour = this.ColourValue;
		}

		/// <summary>
		/// Set the to colour to the current value.
		/// </summary>
		public override void SetEndToCurrentValue() {
			this.toColour = this.ColourValue;
		}

		#endregion

	}
}
