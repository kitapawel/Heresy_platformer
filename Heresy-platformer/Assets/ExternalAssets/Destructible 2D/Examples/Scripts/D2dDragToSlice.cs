using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Destructible2D.Examples
{
	/// <summary>This component allows you to slice all destructible sprites between the mouse down and mouse up points.</summary>
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dDragToSlice")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Drag To Slice")]
	public class D2dDragToSlice : MonoBehaviour
	{
		[Tooltip("The key you must hold down to perform slicing.")]
		public KeyCode Requires = KeyCode.Mouse0;

		[Tooltip("The z position in world space the indicator should spawn at.")]
		public float Intercept;

		[Tooltip("The destructible sprite layers we want to slice.")]
		public LayerMask Layers = -1;

		[Tooltip("The prefab used to show what the slice will look like.")]
		public GameObject IndicatorPrefab;

		[Tooltip("This allows you to change the painting type.")]
		public D2dDestructible.PaintType Paint;

		[Tooltip("The shape of the slice.")]
		public Texture2D Shape;

		[Tooltip("The stamp shape will be multiplied by this.\nSolid White = No Change")]
		public Color Color = Color.white;

		[Tooltip("The thickness of the slice line in world space.")]
		public float Thickness = 1.0f;

		public bool RaycastSlice;

		[Tooltip("If the slice only partially intersects the target object, skip the slice?")]
		public bool SkipPartialSlice;

		public GameObject StartPointPrefab;

		public GameObject EndPointPrefab;

		// Currently slicing?
		[SerializeField]
		private bool down;

		// Mouse position when slicing began
		[SerializeField]
		private Vector3 startMousePosition;

		// Instance of the indicator
		[SerializeField]
		private GameObject indicatorInstance;

		protected virtual void Update()
		{
			// Get the main camera
			var mainCamera = Camera.main;

			// Begin dragging
			if (Input.GetKey(Requires) == true && down == false)
			{
				down               = true;
				startMousePosition = Input.mousePosition;
			}

			// End dragging
			if (Input.GetKey(Requires) == false && down == true)
			{
				down = false;

				// Main camera exists?
				if (mainCamera != null)
				{
					var endMousePosition = Input.mousePosition;
					var startPos         = D2dHelper.ScreenToWorldPosition(startMousePosition, Intercept, mainCamera);
					var endPos           = D2dHelper.ScreenToWorldPosition(  endMousePosition, Intercept, mainCamera);

					D2dSlice.All(Paint, startPos, endPos, Thickness, Shape, Color, Layers);
				}
			}

			// Update indicator?
			if (down == true && mainCamera != null && IndicatorPrefab != null)
			{
				if (indicatorInstance == null)
				{
					indicatorInstance = Instantiate(IndicatorPrefab);
				}

				var startPos   = D2dHelper.ScreenToWorldPosition( startMousePosition, Intercept, mainCamera);
				var currentPos = D2dHelper.ScreenToWorldPosition(Input.mousePosition, Intercept, mainCamera);
				var scale      = Vector3.Distance(currentPos, startPos);
				var angle      = D2dHelper.Atan2(currentPos - startPos) * Mathf.Rad2Deg;

				// Transform the indicator so it lines up with the slice
				indicatorInstance.transform.position   = startPos;
				indicatorInstance.transform.rotation   = Quaternion.Euler(0.0f, 0.0f, -angle);
				indicatorInstance.transform.localScale = new Vector3(Thickness, scale, scale);
			}
			// Destroy indicator?
			else if (indicatorInstance != null)
			{
				Destroy(indicatorInstance.gameObject);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Destructible2D.Examples
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dDragToSlice))]
	public class D2dDragToSlice_Editor : D2dEditor<D2dDragToSlice>
	{
		protected override void OnInspector()
		{
			Draw("Requires");
			Draw("Intercept");
			BeginError(Any(t => t.Layers == 0));
				Draw("Layers");
			EndError();
			BeginError(Any(t => t.IndicatorPrefab == null));
				Draw("IndicatorPrefab");
			EndError();

			Separator();

			Draw("Paint");
			BeginError(Any(t => t.Shape == null));
				Draw("Shape");
			EndError();
			Draw("Color");
			BeginError(Any(t => t.Thickness == 0.0f));
				Draw("Thickness");
			EndError();
			Draw("RaycastSlice");
			if (Any(t => t.RaycastSlice == true))
			{
				BeginIndent();
					Draw("SkipPartialSlice");
					Draw("StartPointPrefab");
					Draw("EndPointPrefab");
				EndIndent();
			}
		}
	}
}
#endif