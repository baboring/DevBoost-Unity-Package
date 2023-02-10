using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Mecani {
	
	/// <summary>
	/// This component introduces some debug behaviours accessible to the animatior's animation event context.
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class MecanimDebug : MonoBehaviour {

		#region Constants

		/// <summary>
		/// String to use to separate the object path to the animation that this controller is on.
		/// </summary>
		private const string PARENT_PATH_SEPARATOR = " > ";

		#endregion

		#region Data

		/// <summary>
		/// String representation of the current hierarchy that this component is in.
		/// </summary>
		private string parentPath = string.Empty;

		#endregion

		#region Monobehaviour

		/// <summary>
		/// Get the initial path of this component and 
		/// </summary>
		private void Start() {
			this.CreateParentPath();
			Debug.LogWarningFormat("Mecanim debug is attached to object at: {0} and should be removed for release.", this.parentPath);
		}

		/// <summary>
		/// When the transform parent changes update the parent path.
		/// </summary>
		private void OnTransformParentChanged() {
			this.CreateParentPath();
		}

		#endregion

		#region Animation Debug Components

		/// <summary>
		/// Calcualtes the hierarchy of the gameobject this component is attached to.
		/// </summary>
		private void CreateParentPath() {
			Stack<string> parentStackName = new Stack<string>();
			Transform transform = this.transform;
			parentStackName.Push(this.gameObject.name);
			while (transform.parent != null) {
				transform = transform.parent;
				parentStackName.Push(transform.name);
			}

			// If we're using the .net 4.0 or higher runtime we have access to new C# methods.
#if CSHARP_7_3_OR_NEWER	
			this.PrintLog(string.Join(PARENT_PATH_SEPARATOR, parentStackName));
#else
			// Otherwise convert to an array and then print.
			this.PrintLog(string.Join(PARENT_PATH_SEPARATOR, parentStackName.ToArray()));
#endif
		}

		/// <summary>
		/// Prints a message via an animation event.
		/// </summary>
		/// <param name="message">Prints a message triggered by an animator value.</param>
		public void PrintLog(string message) {
			Debug.Log(message);
		}

		/// <summary>
		/// Prints a warning via an animation value.
		/// </summary>
		/// <param name="warning">Warning message to print.</param>
		public void PrintWarning(string warning) {
			Debug.LogWarning(warning);
		}

		/// <summary>
		/// Prints an error via an animator value.
		/// </summary>
		/// <param name="error">The error message to print.</param>
		public void PrintError(string error) {
			Debug.LogError(error);
		}

		/// <summary>
		/// This will print the object stack for the component this MecanimController is attached to.
		/// </summary>
		public void PrintObjectStack() {
			Debug.Log(this.parentPath);
		}

		#endregion
	}
}
