using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
/// Useful for providing a raycast target without actually drawing anything.
/// </summary>
public class NonDrawingGraphic : Graphic
{
	/// <summary>
	/// Override the base SetMaterialDirty call, this never needs a material update.
	/// </summary>
	public override void SetMaterialDirty() { return; }

	/// <summary>
	/// Override the base SetVerticesDirty code, this never needs to be redrawn.
	/// </summary>
	public override void SetVerticesDirty() { return; }

	/// <summary>
	/// Include a vertex helper clear in the case of the chain of calls 
	/// `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` happening.
	/// This probably won't happen; so here really just as a fail-safe.
	/// </summary>
	/// <param name="vh">Vertex drawing values for the Graphic, will be cleared because this graphic has no visual representation.</param>
	protected override void OnPopulateMesh( VertexHelper vh )
	{
		vh.Clear();
		return;
	}
}
