using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DevBoost.Mecani {

	/// <summary>
	/// This button subclass will wait for an animation event before sending it's click event.
	/// </summary>
	[AddComponentMenu("UI/Mecanim Button"), RequireComponent(typeof(MecanimController))]
	public class MecanimButton : Button {

		#region Constants

		/// <summary>
		/// Unity event log marker for clicking this button.
		/// </summary>
		private const string UNITY_EVENT_LOG_MARKER = "MechanimButton.onClick";

		#endregion

		#region Data

		/// <summary>
		/// The animation controller that we are listening to for the button animation event.
		/// </summary>
		[SerializeField, Header("----- AnimatedButton -----")]
		private MecanimController buttonAnimator = null;

		/// <summary>
		/// Animation flag to listen for when registering events.
		/// </summary>
		[SerializeField]
		private string animationFlag = string.Empty;
		
		#endregion

		#region MonoBehaviour

// UnityEngine.UI.Selectable has reset wrapped in a #if UNITY_EDITOR so it needs to override if it's in the editor but not otherwise.
#if UNITY_EDITOR

		/// <summary>
		/// Get the animation controller reference for this mecanim button.
		/// </summary>
		protected override void Reset() {
			base.Reset();
			this.buttonAnimator = this.GetComponent<MecanimController>();
			this.transition = Transition.Animation;
		}

#endif

		/// <summary>
		/// Check the required serialized fields.
		/// </summary>
		protected override void Awake() {
			base.Awake();
			Debug.Assert(this.buttonAnimator != null, "Mecanim button needs a mecanim controller to listen to for events.");
			Debug.Assert(!string.IsNullOrEmpty(this.animationFlag), this.gameObject.name + "'s mecanim button needs an animation flag to listen for or it will never complete.");
		}

		#endregion

		#region Button Overrides

		/// <summary>
		/// Calls the mechanim button events. 
		/// Copied from the Press function of UnityEngine.UI.Button.
		/// </summary>
		private void CallButtonEvent() {
			UISystemProfilerApi.AddMarker(UNITY_EVENT_LOG_MARKER, this);
			this.onClick.Invoke();
		}

		/// <summary>
		/// Re-implementation of the press functionality from UnityEngine.UI.Button with a wait for the animation event in the place of calling the event delegate.
		/// </summary>
		private void Press() {
			if (!this.IsActive() || !this.IsInteractable()) {
				return;
			}

			this.buttonAnimator.AddOneShotEvent(this.animationFlag, this.CallButtonEvent);
		}

		/// <summary>
		/// Hide UnityEngine.UI.Button OnPointerClick with our own version that calls the one shot callback object.
		/// </summary>
		/// <param name="eventData">Event data from the pointer click on this button.</param>
		public override void OnPointerClick(PointerEventData eventData) {
			if (eventData.button != PointerEventData.InputButton.Left) {
				return;
			}

			this.Press();
		}

		/// <summary>
		/// Hides the base button's on submit event and calls our press implementation instead of the button's press implementation.
		/// </summary>
		/// <param name="eventData">Event data from the pointer click.</param>
		public override void OnSubmit(BaseEventData eventData) {
			this.Press();

			// If this function is disabled during the press remove the animation event and don't do the transition logic.
			if (!this.IsActive() || !this.IsInteractable()) {
				this.buttonAnimator.RemoveOneShotEvent(this.animationFlag, this.CallButtonEvent);
				return;
			}

			this.DoStateTransition(SelectionState.Pressed, false);
			this.StartCoroutine(this.OnFinishSubmit());
		}

		/// <summary>
		/// Finish submitting by waiting for the state transition to fade to the current base.
		/// </summary>
		private IEnumerator OnFinishSubmit() {
			float fadeTime = colors.fadeDuration;
			float elapsedTime = 0f;

			WaitForEndOfFrame eof = new WaitForEndOfFrame();
			while (elapsedTime < fadeTime) {
				elapsedTime += Time.unscaledDeltaTime;
				yield return eof;
			}

			this.DoStateTransition(this.currentSelectionState, false);
		}

		#endregion

	}
}
