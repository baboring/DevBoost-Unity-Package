using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DevBoost.Mecani {

	/// <summary>
	/// Mecanim controller is a programatic interface to mediate between code and animation settings
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class MecanimController : MonoBehaviour {

		#region Data

		[SerializeField]
		protected Animator animationController = null;
		/// <summary>
		/// The animation controller component holding the mecanim controller state information.
		/// </summary>
		public Animator AnimationController {
			get {
				return this.animationController;
			}
		}

		/// <summary>
		/// Dictionary that is responsible for the 
		/// </summary>
		/// <typeparam name="string">Event key name. Used to map events from the animator to this controller.</typeparam>
		/// <typeparam name="System.Action">The delegate that should be called when the event key is dispatched from the Animator.</typeparam>
		private Dictionary<string, System.Action> eventMap = new Dictionary<string, System.Action>();

		/// <summary>
		/// Dictionary specifically for one shot events.
		/// It's more effecient to just have a parallel memory structure than to wrap a bunch of delegate functions to ensure that they're removed on call.
		/// </summary>
		/// <typeparam name="string">Trigger key</typeparam>
		/// <typeparam name="System.Action">Events that should only respond to the trigger once.</typeparam>
		private Dictionary<string, System.Action> oneShotEventMap = new Dictionary<string, System.Action>();

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// Try and get the animatior attached to the same object as this at the 
		/// </summary>
		protected virtual void Reset() {
			this.animationController = this.GetComponent<Animator>();
		}

		#endregion

		#region MechanimController Event management

		/// <summary>
		/// Method that will recieve the animation event from the animation controller.
		/// If we have any mapped events we check the dictionaries and call the delegates they have passed.
		/// </summary>
		/// <param name="animationEventKey">The animation event key to use for the key.</param>
		public void AnimationEvent(string animationEventKey) {
			Action animationEventAction = null;
			// One shot events will be called before repeating events.
			if (this.oneShotEventMap.TryGetValue(animationEventKey, out animationEventAction)) {
				animationEventAction.Invoke();
				this.oneShotEventMap.Remove(animationEventKey);
			}
			
			animationEventAction = null;
			if (this.eventMap.TryGetValue(animationEventKey, out animationEventAction)) {
				animationEventAction.Invoke();
			}

		}

		/// <summary>
		/// Adds an event that will be triggered by the given event key.
		/// This event will be called every time the flag value is sent to this MecanimController.
		/// </summary>
		/// <param name="eventKey">Key for the event to add.</param>
		/// <param name="eventDelegate">The delegate to be called when the event key is hit.</param>
		public void AddEvent(string eventKey, Action eventDelegate) {
			this.AddEventInternal(eventKey, eventDelegate, this.eventMap);
		}

		/// <summary>
		/// Removes a repeating event so that it will not be called anymore.
		/// </summary>
		/// <param name="eventKey">The key of the event to remove.</param>
		/// <param name="eventDelegate">The delegate to remove.</param>
		public void RemoveEvent(string eventKey, Action eventDelegate) {
			if (this.eventMap.ContainsKey(eventKey)) {
				Action mappedDelegate = this.eventMap[eventKey];
				mappedDelegate -= eventDelegate;

				// If the invocation list is length 0 now pull the event delegate off the list and 
				if (mappedDelegate == null || mappedDelegate.GetInvocationList().Length == 0) {
					this.eventMap.Remove(eventKey);
				}
			}
		}

		/// <summary>
		/// Adds an event that will be called only once and never again 
		/// </summary>
		/// <param name="eventKey">The event key used to trigger the one shot event.</param>
		/// <param name="oneShotEventDelegate">Delegate event to trigger only once.</param>
		public void AddOneShotEvent(string eventKey, Action oneShotEventDelegate) {
			this.AddEventInternal(eventKey, oneShotEventDelegate, this.oneShotEventMap);
		}

		/// <summary>
		/// This will remove a one shot event from a trigger without firing it.
		/// </summary>
		/// <param name="eventKey">The event key to remove the matching delegate from.</param>
		/// <param name="oneShotEventDelegate">Delegate to remove from the one shot list.</param>
		public void RemoveOneShotEvent(string eventKey, Action oneShotEventDelegate) {
			this.RemoveEventInternal(eventKey, oneShotEventDelegate, this.oneShotEventMap);
		}

		/// <summary>
		/// Convenience function that will remove all the events on this controller.
		/// </summary>
		public void RemoveAllEvents() {
			this.eventMap.Clear();
			this.oneShotEventMap.Clear();
		}

		/// <summary>
		/// This function will actually adding an event in a structured way to a given event map.
		/// </summary>
		/// <param name="eventKey">The event key to listen for.</param>
		/// <param name="eventDelegate">Delegate to call when the event is triggered.</param>
		/// <param name="eventDict">Dictionary to add the event to.</param>
		private void AddEventInternal(string eventKey, Action eventDelegate, Dictionary<string, Action> eventDict) {
			if (eventDict.ContainsKey(eventKey)) {
				// If there is already a delegate listening for this key just add the new event delegate to the existing delegate.
				eventDict[eventKey] += eventDelegate;
			} else {
				// Otherwise use the new delegate as the base delegate.
				eventDict[eventKey] = eventDelegate;
			}
		}

		/// <summary>
		/// This function will remove a delegate from an event in a structured way from a specified event map. 
		/// </summary>
		/// <param name="eventKey">Event key to remove the event from listening to.</param>
		/// <param name="eventDelegate">Delegate to remove.</param>
		/// <param name="eventDict">The event dictionary </param>
		private void RemoveEventInternal(string eventKey, Action eventDelegate, Dictionary<string, Action> eventDict) {
			if (eventDict.ContainsKey(eventKey)) {
				Action mappedDelegate = this.eventMap[eventKey];
				mappedDelegate -= eventDelegate;

				// If the invocation list is length 0 now pull the event delegate off the list and 
				if (mappedDelegate.GetInvocationList().Length == 0) {
					eventDict.Remove(eventKey);
				}
			}
		}

		#endregion

		#region Animation Control

		/// <summary>
		/// Sets the value of an animator boolean.
		/// </summary>
		/// <param name="booleanID">The id of the animation boolean value to set.</param>
		/// <param name="value">Value to set the boolean to.</param>
		public void SetBoolean(string booleanID, bool value) {
			this.animationController.SetBool(booleanID, value);
		}

		/// <summary>
		/// Toggle a boolean value.
		/// </summary>
		/// <param name="booleanID">The ID of the boolean value to toggle.</param>
		public void Toggle(string booleanID) {
			this.animationController.SetBool(booleanID, !this.animationController.GetBool(booleanID));
		}

		/// <summary>
		/// Sets a trigger active on the Animator.
		/// </summary>
		/// <param name="triggerID">Mecanim id of the trigger that should be triggered.</param>
		public void Trigger(string triggerID) {
			this.animationController.SetTrigger(triggerID);
		}

		/// <summary>
		/// Resets a trigger active on the Animator.
		/// </summary>
		/// <param name="triggerID">Mecanim id of the trigger that should be reset.</param>
		public void ResetTrigger(string triggerID) {
			this.animationController.ResetTrigger(triggerID);
		}

		/// <summary>
		/// Sets a float to a specific value.
		/// </summary>
		/// <param name="floatID">The id of the float to set.</param>
		/// <param name="value">The value to set the float to.</param>
		public void SetFloat(string floatID, float value) {
			this.animationController.SetFloat(floatID, value);
		}

		/// <summary>
		/// Sets an integer value on the animator being controlled by the mecanim controller.
		/// </summary>
		/// <param name="integerId">The id of the integer to set.</param>
		/// <param name="value">The value to set the integer to.</param>
		public void SetInt(string integerId, int value) {
			this.animationController.SetInteger(integerId, value);
		}

		/// <summary>
		/// Accessor to set the float to a random value between 0  and 1 [inclusive].
		/// This can be used by art to run some basic randomized animations off of the animator.
		/// </summary>
		/// <param name="floatID">ID of the float value to randomize</param>
		public void SetRandomFloat(string floatID) {
			this.animationController.SetFloat(floatID, UnityEngine.Random.value);
		}

		#endregion

		#region Object

		/// <summary>
		/// Prints out a summary of the events attached to this mecanim controller.
		/// </summary>
		/// <returns>A string representation of the events attached to this mecanim controller.</returns>
		public override string ToString() {
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendFormat("MecanimController on {0}:\n", this.name);
			foreach (string key in this.eventMap.Keys) {
				stringBuilder.AppendFormat("Key: {0} has {1} events registered.", key, this.eventMap[key].GetInvocationList().Length);
				stringBuilder.AppendLine();
			}

			return stringBuilder.ToString();
		}

		#endregion

	}
}
