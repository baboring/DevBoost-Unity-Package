using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevBoost.Effects {

	/// <summary>
	/// This is essentially the standard unity Outline mesh effect component with an option for colourized outlines instead of a settings outline.
	/// </summary>
	[AddComponentMenu("UI/Effects/UIOutline", 16)]
	public class UIOutline : BaseMeshEffect {

		#region Constants

		/// <summary>
		/// Number of vertices making up the defintion of a rect transform.
		/// </summary>
		private const int RECT_TRANSFORM_CORNER_COUNT = 4;

		/// <summary>
		/// Value to be used as a flag in the UV2 component of the UI to signal to supported shaders to flat-colour the outline.
		/// </summary>
		private const float OUTLINE_UV_FLAG = 1f;

		#endregion
		
		#region Data

		[SerializeField]
		private Color outlineColour = Color.white;
		/// <summary>
		/// Colour to set the outline colour to.
		/// </summary>
		public Color OutlineColour {
			get {
				return this.outlineColour;
			}

			set {
				this.outlineColour = value;

				if (this.graphic != null) {
					this.graphic.SetVerticesDirty();
				}
			}
		}

		[SerializeField]
		private Vector2 outlineSize = Vector2.zero;
		/// <summary>
		/// The desired offset positions of the shadow clones used for 
		/// </summary>
		public Vector2 OutlineSize {
			get {
				return this.outlineSize;
			}
			set {
				this.outlineSize = value;
				if (this.graphic != null) {
					this.graphic.SetVerticesDirty();
				}
			}
		}

		/// <summary>
		/// If set to true multiply blend the graphic with the alpha.
		/// </summary>
		[SerializeField]
		private bool useGraphicAlpha = true;

		/// <summary>
		/// Use the constant centre value.
		/// </summary>
		[SerializeField]
		private bool improveOutline = true;

		#endregion

		#region Constructor

		/// <summary>
		/// Unity BaseMeshEffects tend to all have a protected default constructor so following that standard.
		/// </summary>
		protected UIOutline() { }

		#endregion

		#region MonoBehaviour

#if UNITY_EDITOR

	/// <summary>
	/// When a value in the editor changes update the vertices of the outline.
	/// Only have to do this during editor time.
	/// </summary>
	protected override void OnValidate() {
		if (graphic != null) {
			graphic.SetVerticesDirty();
		}

		base.OnValidate();
	}

#endif

		#endregion

		#region UIOutline

		/// <summary>
		/// Function to add the outline verticies to the list of UIVertexes that make up the UI component that is being created.
		/// </summary>
		/// <param name="verts">List of Vertices that make up the mesh of the item we are outlining.</param>
		/// <param name="start">The start index of the vertcies to outline.</param>
		/// <param name="end">The end index of the verticies to outline.</param>
		protected void AddOutlineVerts(List<UIVertex> verts, int start, int end, float x, float y) {
			UIVertex tempVert;

			var neededCapacity = verts.Count + end - start;
			if (verts.Capacity < neededCapacity) {
				verts.Capacity = neededCapacity;
			}

			for (int i = start; i < end; ++i) {
				tempVert = verts[i];
				verts.Add(tempVert);

				tempVert.position.x += x;
				tempVert.position.y += y;

				// Cast the outline colour as a colour32
				Color32 newColor = this.OutlineColour;

				if (this.useGraphicAlpha) {
					// Need to convert the alpha value to a byte value because the shader system takes Color32s.
					newColor.a = (byte)((newColor.a * verts[i].color.a) / 255);
				}

				tempVert.uv2 = new Vector2(OUTLINE_UV_FLAG, 0f);
				tempVert.color = newColor;
				verts[i] = tempVert;
			}
		}

		/// <summary>
		/// Handles the mesh modification command coming from the 
		/// </summary>
		/// <param name="vertexHelper">The vertex helper component containing the graphic verticies.</param>
		public override void ModifyMesh(VertexHelper vertexHelper) {
			/// If the component isn't active bail on the mesh modification.
			if (!this.IsActive()) {
				return;
			}

			// Otherwise get a list of UIVertex attributes and add the outline vertices to them.
			List<UIVertex> output = ListPool<UIVertex>.Get();
			vertexHelper.GetUIVertexStream(output);

			int start = 0;
			int end = output.Count;
			this.AddOutlineVerts(output, start, end, outlineSize.x, outlineSize.y);
			start = end;
			end = output.Count;
			this.AddOutlineVerts(output, start, end, outlineSize.x, -outlineSize.y);
			start = end;
			end = output.Count;
			this.AddOutlineVerts(output, start, end, -outlineSize.x, outlineSize.y);
			start = end;
			end = output.Count;
			this.AddOutlineVerts(output, start, end, -outlineSize.x, -outlineSize.y);

			if (this.improveOutline) {
				start = end;
				end = output.Count;
				this.AddOutlineVerts(output, start, end, outlineSize.x, 0f);
				start = end;
				end = output.Count;
				this.AddOutlineVerts(output, start, end, -outlineSize.x, 0f);
				start = end;
				end = output.Count;
				this.AddOutlineVerts(output, start, end, 0f, outlineSize.y);
				start = end;
				end = output.Count;
				this.AddOutlineVerts(output, start, end, 0f, -outlineSize.y);
			}
			
			vertexHelper.Clear();
			vertexHelper.AddUIVertexTriangleStream(output);
			ListPool<UIVertex>.Release(output);
		}

		#endregion

	}

}
