using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

#if UNITY_2018_3_OR_NEWER
	// Unity 2018.3 and up use the new prefab system which is not technically in edit mode so ExecuteInEditMode is being phased out.
	[ExecuteAlways]
#else
	[ExecuteInEditMode]
#endif
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class NoJitterContentSizeFitter : UIBehaviour, ILayoutSelfController
{

	#region Constant

	/// <summary>
	/// Flag value for treating the content fitter lerp speed as instant.
	/// </summary>
	private const float INSTANT = -1.0f;

	#endregion

	#region Enum

	/// <summary>
	/// Enumeration of fitting behaviours avaliable to this content size fitter.
	/// </summary>
	public enum ContentSizeType
	{
		Unconstrained,
		PreferredSize,
		AlwaysGrow,
		AlwaysShrink
	}

	#endregion
	
	#region Data

	/// <summary>
	/// How should the content fit itself horizontally.
	/// </summary>
	[SerializeField]
	protected ContentSizeType horizontalFit = ContentSizeType.Unconstrained;

	/// <summary>
	/// How should the content fit itself vertically.
	/// </summary>
	[SerializeField]
	protected ContentSizeType verticalFit = ContentSizeType.Unconstrained;

	/// <summary>
	/// Multiplier to control the lerp time that smooths out the constrained size.
	/// -1.0 should behave similarly to a standard ContentSizeFitter.
	/// </summary>
	[SerializeField]
	[Tooltip("Multiplier to delta time for how quickly to slide the width. -1.0 = instant ")]
	private float lerpSpeed = 5.0f;

	/// <summary>
	/// The minimum horizontal size of the content.
	/// </summary>
	private float minWidth = float.MaxValue;
	
	/// <summary>
	/// The maximum horizontal size of the content.
	/// </summary>
	private float maxWidth = float.MinValue;

	/// <summary>
	/// The minimum vertical size of the content.
	/// </summary>
	private float minHeight = float.MaxValue;

	/// <summary>
	/// The maximum vertical size of the content.
	/// </summary>
	private float maxHeight = float.MinValue;

	/// <summary>
	/// The desired horizontal size that the fitter is lerping towards.
	/// </summary>
	private float targetWidth = float.MinValue;

	/// <summary>
	/// The desiered vertical size that the fitter is lerping towards.
	/// </summary>
	private float targetHeight = float.MinValue;

	[System.NonSerialized] 
	private RectTransform m_Rect;
	/// <summary>
	/// The rect transform that is being controlled.
	/// </summary>
	private RectTransform rectTransform
	{
		get
		{
			if (m_Rect == null)
			{
				this.m_Rect = GetComponent<RectTransform>();
			}
			return this.m_Rect;
		}
	}

	/// <summary>
	/// List of the immediate child transforms of this ContentSizeFitter.
	/// </summary>
	private List<RectTransform> childrenTransforms = null;

	/// <summary>
	/// This tracks the rect transforms that this ContentSizeFitter is currently driving.
	/// </summary>
	private DrivenRectTransformTracker m_Tracker;

	#endregion

	/// <summary>
	/// When this component is enabled we gather a list of it's child transforms so we can resize according to them.
	/// </summary>
	protected override void OnEnable()
	{
		base.OnEnable();
		this.childrenTransforms = new List<RectTransform>();
		for (int i = 0; i < this.gameObject.transform.childCount; i++)
		{
			this.childrenTransforms.Add(this.gameObject.transform.GetChild(i).GetComponent<RectTransform>());
		}
		
		this.SetDirty();
	}

	/// <summary>
	/// When this component is disabled mark the controlled transform for a rebuild.
	/// </summary>
	protected override void OnDisable()
	{
		this.m_Tracker.Clear();
		LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		base.OnDisable();
	}

	/// <summary>
	/// Standard set dirty when the size changes so we get re-laid out.
	/// </summary>
	protected override void OnRectTransformDimensionsChange()
	{
		this.SetDirty();
	}

	/// <summary>
	/// On update check if the size values are withing the correct value and use lerp to smooth out the changes in size.
	/// </summary>
	private void Update()
	{
		if (this.targetWidth != float.MinValue && targetHeight != float.MinValue)
		{
			if (this.lerpSpeed == INSTANT)
			{
				this.rectTransform.sizeDelta = new Vector2(this.targetWidth, this.targetHeight);
			}
			else
			{
				float newWidth = Mathf.Lerp(this.rectTransform.sizeDelta.x, this.targetWidth, Time.unscaledDeltaTime * this.lerpSpeed);
				float newHeight = Mathf.Lerp(this.rectTransform.sizeDelta.y, this.targetHeight, Time.unscaledDeltaTime * this.lerpSpeed);
				this.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
			}
			
		}
	}

	/// <summary>
	/// Handle the size fitting logic and update values for the update step.
	/// </summary>
	private void HandleSelfFittingAlongAxis()
	{
		
#if UNITY_EDITOR

		if (!Application.isPlaying)
		{
			this.minWidth = float.MaxValue;
			this.maxWidth = float.MinValue;
			this.minHeight = float.MaxValue;
			this.maxHeight = float.MinValue;
		}

#endif

		//find the current width and height of the children
		float currentWidth = 0.0f;
		float currentHeight = 0.0f;
		foreach(RectTransform rt in childrenTransforms)
		{
			currentWidth += rt.rect.width;
			currentHeight += rt.rect.height;
			
		}

		// Handle Horizontal fit.
		switch (this.horizontalFit)
		{
		case ContentSizeType.AlwaysGrow:
			if (currentWidth > this.maxWidth)
			{
				this.maxWidth = currentWidth;
				this.targetWidth = this.maxWidth;
			}
			break;
		case ContentSizeType.AlwaysShrink:
			if (currentWidth < minWidth)
			{
				this.minWidth = currentWidth;
				this.targetWidth = this.minWidth;
			}
			break;
		case ContentSizeType.PreferredSize:
			{
				this.targetWidth = currentWidth;
			}
			break;
		case ContentSizeType.Unconstrained:
			{
				this.targetWidth = this.rectTransform.sizeDelta.x;
			}
			break;
		}

		// Vertical fit.
		switch (this.verticalFit)
		{
		case ContentSizeType.AlwaysGrow:
			if (currentHeight > maxHeight)
			{
				this.maxHeight = currentHeight;
				this.targetHeight = currentHeight;
			}
			break;
		case ContentSizeType.AlwaysShrink:
			if (currentHeight < minHeight)
			{
				this.minHeight = currentHeight;
				this.targetHeight = currentHeight;
			}
			break;
		case ContentSizeType.PreferredSize:
			{
				this.targetHeight = currentHeight;
			}
			break;
		case ContentSizeType.Unconstrained:
			{
				this.targetHeight = this.rectTransform.sizeDelta.y;
			}
			break;
		}
	}

	/// <summary>
	/// Called by the layout system when setting the horizontal size.
	/// </summary>
	public virtual void SetLayoutHorizontal()
	{
		this.m_Tracker.Clear();
		this.HandleSelfFittingAlongAxis();
	}

	/// <summary>
	/// Called by the layout system when setting the vertical size.
	/// </summary>
	public virtual void SetLayoutVertical()
	{
		this.HandleSelfFittingAlongAxis();
	}

	/// <summary>
	/// Marks this layout as dirty and sets it for a rebuild.
	/// </summary>
	protected void SetDirty()
	{
		if (this.IsActive())
		{
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}
	}

#if UNITY_EDITOR

	/// <summary>
	/// In editor just set the layout dirty whenever any value changes.
	/// </summary>
	protected override void OnValidate()
	{
		this.SetDirty();
	}

#endif

}
