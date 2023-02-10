using UnityEditor;
using UnityEditor.UI;

namespace DevBoost.Mecani {

	/// <summary>
	/// Editor for Animation MecanimButton.
	/// </summary>
	[CustomEditor(typeof(MecanimButton), true), CanEditMultipleObjects]
	public class MecanimButtonEditor : ButtonEditor {

		#region Constants

		/// <summary>
		/// The name of the animation variable in MecanimButton.
		/// </summary>
		private const string ANIMATOR_PROPERTY_NAME = "buttonAnimator";

		/// <summary>
		/// The name of the animation flag variable in MecanimButton.
		/// </summary>
		private const string ANIM_FLAG_PROPERTY_NAME = "animationFlag";

		#endregion

		#region Data

		/// <summary>
		/// Property object for the mecanim component.
		/// </summary>
		private SerializedProperty buttonAnimatorProperty = null;

		/// <summary>
		/// Property field for the animations flag property.
		/// </summary>
		private SerializedProperty animationFlagProperty = null;

		#endregion

		#region Button Editor

		/// <summary>
		/// Find the properties from the MecanimButton script when enabled.
		/// </summary>
		protected override void OnEnable() {
			base.OnEnable();
			this.buttonAnimatorProperty = this.serializedObject.FindProperty(ANIMATOR_PROPERTY_NAME);
			this.animationFlagProperty = this.serializedObject.FindProperty(ANIM_FLAG_PROPERTY_NAME);
		}

		/// <summary>
		/// Draw the new components at the top of the editor and then draw the rest of the standard button components.
		/// </summary>
		public override void OnInspectorGUI() {
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.buttonAnimatorProperty);
			EditorGUILayout.PropertyField(this.animationFlagProperty);
			this.serializedObject.ApplyModifiedProperties();

			EditorGUILayout.Space();
			base.OnInspectorGUI();
		}

		#endregion
	}
}