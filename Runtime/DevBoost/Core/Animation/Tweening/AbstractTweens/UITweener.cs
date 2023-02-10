using UnityEngine;

namespace DevBoost.Mecani {

	/// <summary>
	/// A component based tweener that can be set up in the editor.
	/// The functionality of this is based on UITweener from the NGUI library for unity versions pre-UGUI.
	/// </summary>
	public abstract class UITweener : MonoBehaviour {

		#region Constants

		/// <summary>
		/// A default amount per delta, this will complete the tween in a single timestep by default.
		/// </summary>
		private const float DEFAULT_AMOUNT_PER_DELTA = 1f;

		#endregion

		#region Enums

		/// <summary>
		/// The tween mode determines how the tween will behave when it reaches it's end.
		/// </summary>
		public enum TweenMode {
			/// <summary>
			/// The tween will play once through in the correct direction and then disable itself.
			/// </summary>
			Once,
			/// <summary>
			/// This tween will loop 
			/// </summary>
			Loop,
			/// <summary>
			/// A component based in engine tweener for 
			/// </summary>
			PingPong
		}

		#endregion

		#region Data

		/// <summary>
		/// Animation curve for the tween to follow.
		/// </summary>
		/// <returns></returns>
		[SerializeField, Header("----- UI Tweener -----")]
		private AnimationCurve tweenCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		/// <summary>
		/// The tween mode will change how this tween behaves
		/// </summary>
		[SerializeField]
		private TweenMode tweenMode = TweenMode.Once;

		[SerializeField, Tooltip("The length in seconds for this tween to take.")]
		private float tweenLength = 1f;
		/// <summary>
		/// The length in seconds for this tween to take.
		/// </summary>
		public float TweenLength {
			get {
				return this.tweenLength;
			}
		}

		[SerializeField]
		private float startDelay = 0f;
		/// <summary>
		/// Time, in seconds to delay the tween from it's initialization time.
		/// </summary>
		public float StartDelay {
			get {
				return this.startDelay;
			}
		}

		/// <summary>
		/// Flag indicating that the tween should use real time instead of frame time to update.
		/// </summary>
		[SerializeField]
		private bool useRealTime = false;

		/// <summary>
		/// Flag indicating that the tween should reset and play itself forward automatically when it is enabled.
		/// </summary>
		[SerializeField]
		private bool autoPlayOnEnable = false;

		[SerializeField, Tooltip("Called when the tween is completed. Only called during TweenMode.Once.")]
		private UnityEngine.Events.UnityEvent onComplete = null;
		/// <summary>
		/// This function will be called when the tween is completed. 
		/// The tween component will still be active when this callback is called.
		/// It will only be called during TweenMode.Once
		/// </summary>
		public UnityEngine.Events.UnityEvent OnComplete {
			get {
				return this.onComplete;
			}
		}

		[SerializeField, Tooltip("Called whenever the tween loops or changes direction. Called during TweenMode.Loop and TweenMode.PingPong.")]
		private UnityEngine.Events.UnityEvent onLoop = null;
		/// <summary>
		/// Called whenever the tweener reaches the end of it's tween and resets or changes direction.
		/// </summary>
		public UnityEngine.Events.UnityEvent OnLoop {
			get {
				return this.onLoop;
			}
		}

		[SerializeField, Tooltip("Called whenever the tween gets it's value updated.")]
		private UnityEngine.Events.UnityEvent onValueUpdated = null;
		/// <summary>
		/// Called whenever the tween value updates.
		/// </summary>
		public UnityEngine.Events.UnityEvent OnValueUpdated {
			get {
				return this.onValueUpdated;
			}
		}

		/// <summary>
		/// The time to consider the "start" of the tween.
		/// </summary>
		private float startTime = 0f;

		/// <summary>
		/// The current normalized tweening factor. 
		/// Always between 0 and 1, 0 representing the start of the tween, 1 representing the end of the tween.
		/// </summary>
		private float tweenFactor = 0f;

		/// <summary>
		/// The current duration, taking into account the direction of travel.
		/// </summary>
		private float currentDuration = 0f;

		private float amountPerDelta = 1f;
		/// <summary>
		/// Calculates the amount changed per delta time.
		/// This can change whenever the tween length updates.
		/// </summary>
		public float AmountPerDelta {
			get {
				if (this.currentDuration != this.tweenLength) {
					this.currentDuration = this.tweenLength;
					// Save the direction of the delta.
					float sign = Mathf.Sign(this.amountPerDelta);

					// Reset the amount per delta.
					this.amountPerDelta = DEFAULT_AMOUNT_PER_DELTA;
		
					if (this.tweenLength > 0f) {
						this.amountPerDelta = Mathf.Abs(1f / this.tweenLength);
					}

					this.amountPerDelta *= sign;
				}

				return this.amountPerDelta;
			}
		}

		/// <summary>
		/// Flag indicating that the tween has started and tween events should be processed.
		/// </summary>
		private bool started = false;

		/// <summary>
		/// Flag indicating that the UITweener has finished.
		/// </summary>
		private bool isFinished = false;

		private bool updating = false;
        /// <summary>
		/// Flag indicating that this tween is currently updating.
		/// </summary>
        public bool Updating
        {
            get { return this.updating; }
        }

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// Disable the tween logic when the component is disabled.
		/// </summary>
		protected virtual void OnDisable() {
			this.started = false;
			this.updating = false;
		}

		/// <summary>
		/// If the tween is set up to autoplay when it's enabled we should reset it and play it when it starts.
		/// </summary>
		protected virtual void OnEnable() {
			if (this.autoPlayOnEnable) {
				this.ResetToBeginning(true);
			}
		}

		/// <summary>
		/// Ensure that the update function is called on the frame that the tween begins on.
		/// </summary>
		protected virtual void Start() {
			this.Update();
		}

		/// <summary>
		/// When added to a component call the set values.
		/// </summary>
		protected virtual void Reset() {
			if (!this.started) {
				this.SetStartToCurrentValue();
				this.SetEndToCurrentValue();

				this.ResetToBeginning();
			}
		}

		/// <summary>
		/// Run the tweening logic in update.
		/// </summary>
		protected virtual void Update() {
			if (!this.updating) {
				return;
			}

			float frameDeltaTime = this.useRealTime ? Time.unscaledDeltaTime : Time.deltaTime;
			float time = this.useRealTime ? Time.unscaledTime : Time.time;

			if (!this.started) {
				this.started = true;
				
				this.startTime = time + this.startDelay;
			}

			// Don't process any tween updates until the start time has come.
			if (time < this.startTime) {
				return;
			}

			// Calculate the new tween factor.
			this.tweenFactor += this.AmountPerDelta * frameDeltaTime;

			this.isFinished = false;
			// Check if the tween has finished.
			switch (this.tweenMode) {
			case TweenMode.Once:
				if (this.tweenFactor > 1f || this.tweenFactor < 0f) {
					// Discard any overflow.
					this.tweenFactor = Mathf.Clamp01(this.tweenFactor);
					this.isFinished = true;

					// Manually sample to set the tween to the completed end.
					this.Sample();

					// Call oncomplete before disabling the Tweener.
					if (this.OnComplete != null) {
						this.OnComplete.Invoke();
					}

					this.updating = false;
				}

				break;
			case TweenMode.Loop:
				if (this.tweenFactor > 1f) {
					// Reset the tween factor to near 0, keeping any remainder.
					this.tweenFactor -= Mathf.Floor(this.tweenFactor);
					// Call any functions listening for looping.
					if (this.OnLoop != null) {
						this.OnLoop.Invoke();
					}
				}

				break;
			case TweenMode.PingPong:
				if (this.tweenFactor > 1f) {
					// Set the tween factor to the remainder of whatever overtween we got.
					this.tweenFactor = 1f - (this.tweenFactor - Mathf.Floor(this.tweenFactor));
					
					// Reverse the tween direction.
					this.amountPerDelta *= -1f;
				} else if (this.tweenFactor < 0f) {
					// Invert the tween factor so the remainder is above zero.
					this.tweenFactor *= -1f;
					this.tweenFactor -= Mathf.Floor(this.tweenFactor);

					// Reverse direction.
					this.amountPerDelta *= -1;
				}

				break;
			default:
				Debug.LogError("Do not recognize TweenMode: " + this.tweenMode.ToString());

				break;
			}

			// If the tweener didn't finish this frame sample the tween value and continue the tweening process.
			if (!isFinished) {
				this.Sample();
			}
		}

		#endregion

		#region UITweener

		/// <summary>
		/// Play this tween.
		/// </summary>
		/// <param name="playForward">If set to <c>true</c> play the tween forward, otherwise play it backwards.</param>
		public void Play(bool playForward = true) {
			this.amountPerDelta = Mathf.Abs(this.AmountPerDelta);
			if (!playForward) {
				this.amountPerDelta *= -1f;
			}

			this.updating = true;
		}

		/// <summary>
		/// Resets the tween to the beginning.
		/// </summary>
		/// <param name="forceEnableTween">If set to <c>True</c> enable the tween component after the reset, otherwise leave the state be.</param>
		public void ResetToBeginning(bool forceEnableTween = false) {
			this.started = false;

			// If we are forcing the tween to be enabled we set it enabled. Otherwise leave it in it's current state.
			if (forceEnableTween) {
				this.updating = true;
			}

			this.tweenFactor = (this.AmountPerDelta < 0f) ? 1f : 0f;
			this.Sample();
		}

		/// <summary>
		/// Handles sampling the tween factor from the animation curve if there is one present and passing the value to the TweenUpdate function.
		/// </summary>
		private void Sample() {
			float sampleValue = this.tweenFactor;
			if (this.tweenCurve != null) {
				sampleValue = this.tweenCurve.Evaluate(sampleValue);
			}

			this.TweenUpdate(sampleValue, this.isFinished);

			if (this.onValueUpdated != null) {
				this.onValueUpdated.Invoke();
			}
		}

		/// <summary>
		/// Sets the tween factor to zero.
		/// </summary>
		public virtual void SetTweenToStart() {
			this.tweenFactor = 0f;

			float sampleValue = this.tweenFactor;
			if (this.tweenCurve != null) {
				sampleValue = this.tweenCurve.Evaluate(sampleValue);
			}

			this.TweenUpdate(sampleValue, true);
			
			if (this.onValueUpdated != null) {
				this.onValueUpdated.Invoke();
			}
		}

		/// <summary>
		/// Sets the tween factor directly to 1.
		/// </summary>
		public virtual void SetTweenToEnd() {
			this.tweenFactor = 1f;

			float sampleValue = this.tweenFactor;
			if (this.tweenCurve != null) {
				sampleValue = this.tweenCurve.Evaluate(sampleValue);
			}

			this.TweenUpdate(sampleValue, true);

			if (this.onValueUpdated != null) {
				this.onValueUpdated.Invoke();
			}
		}

		/// <summary>
		/// Handle the specific tweening operation.
		/// </summary>
		/// <param name="factor">Calculated tween factor to use for tweening.</param>
		/// <param name="isFinished">Flag indicating if the tween operation finished in this frame.</param>
		protected abstract void TweenUpdate(float factor, bool isFinished);

		/// <summary>
		/// Sets the start value of this tweener to the current value.
		/// </summary>
		[EditorButton]
		public abstract void SetStartToCurrentValue();

		/// <summary>
		/// Sets the end value to the current value of the UITweener.
		/// </summary>
		[EditorButton]
		public abstract void SetEndToCurrentValue();
		
		/// <summary>
		/// Sets how long this Tweener's tween animation will take.
		/// </summary>
		/// <param name="newLength">The new length of the tween.</param>
		public void SetTweenLength(float newLength) {
			this.tweenLength = Mathf.Abs(newLength);
		}
 
		/// <summary>
		/// Sets how long to delay this Tweener's tween animation will take.
		/// </summary>
		/// <param name="newDelay">The new start delay for this tween.</param>
		public void SetTweenStartDelay(float newDelay) {
			this.startDelay = Mathf.Abs(newDelay);
		}

		#endregion

		#region Static

		/// <summary>
		/// Manually start a tweening operation of a tween on a specific game object.
		/// </summary>
		/// <param name="tweenTarget">The game object to run the tween on.</param>
		/// <param name="duration">Duration of the tween.</param>
		/// <typeparam name="T">The tween type to access. If a tween of that type is not currently on the </typeparam>
		/// <returns></returns>
		static public T StartTween<T>(GameObject tweenTarget, float duration) where T : UITweener {
			T tween = tweenTarget.GetComponent<T>();

			if (tween == null) {
				tween = tweenTarget.AddComponent<T>();
			}

			tween.started = false;
			tween.tweenLength = duration;

			// Manual setup because start might not be called.
			tween.amountPerDelta = Mathf.Abs(tween.AmountPerDelta);
			tween.updating = true;

			return tween;
		}

		#endregion
	}
}
