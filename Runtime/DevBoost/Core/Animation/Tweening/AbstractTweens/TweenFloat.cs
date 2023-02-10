using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Mecani {

	/// <summary>
	/// Abstract tweener that handles tweening a single floating point value.
	/// </summary>
	public abstract class TweenFloat : UITweener {

		#region Data

		/// <summary>
		/// Tween a single floating point value.
		/// </summary>
		[SerializeField, Header("----- Tween Float Value -----")]
		private float from = 0f;

		/// <summary>
		/// Value to tween to.
		/// </summary>
		[SerializeField]
		private float to = 0f;

		/// <summary>
		/// The floating point value to tween.
		/// </summary>
		protected abstract float Value {
			get; set;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public override void SetStartToCurrentValue() {
			this.from = this.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void SetEndToCurrentValue() {
			this.to = this.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="factor"></param>
		/// <param name="isFinished"></param>
		protected override void TweenUpdate(float factor, bool isFinished) {
			this.Value = Mathf.LerpUnclamped(this.from, this.to, factor);
		}
	}
}
