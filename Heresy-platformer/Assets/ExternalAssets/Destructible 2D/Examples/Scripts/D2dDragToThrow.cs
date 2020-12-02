using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Destructible2D.Examples
{
	/// <summary>This component spawns and throws a prefab when you click and drag across the screen.</summary>
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dDragToThrow")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Drag To Throw")]
	public class D2dDragToThrow : MonoBehaviour
	{
		[Tooltip("The key you must hold down to do slicing")]
		public KeyCode Requires = KeyCode.Mouse0;

		[Tooltip("The z position the indicator should spawn at")]
		public float Intercept;

		[Tooltip("The prefab used to show what the slice will look like")]
		public GameObject IndicatorPrefab;

		[Tooltip("The scale of the throw indicator")]
		public float Scale = 1.0f;

		[Tooltip("The minimum distance the throw is calculated using in world space.\n\n0 = Unlimited.")]
		public float DistanceMin;

		[Tooltip("The maximum distance the throw is calculated using in world space.\n\n0 = Unlimited.")]
		public float DistanceMax;

		[Tooltip("The prefab that gets thrown")]
		public GameObject ProjectilePrefab;

		[Tooltip("How fast the projectile will be launched")]
		public float ProjectileSpeed = 10.0f;

		[Tooltip("How much spread is added to the project when fired")]
		public float ProjectileSpread;

		[Tooltip("The projectile will be rotated by this angle in degrees.")]
		public float ProjectileAngle;

		// Currently slicing?
		[SerializeField]
		private bool down;

		// Mouse position when slicing began
		[SerializeField]
		private Vector3 startMousePosition;

		// Instance of the indicator
		[SerializeField]
		private GameObject indicatorInstance;

		private float GetAngleAndClampCurrentPos(Vector3 startPos, ref Vector3 currentPos)
		{
			if (startPos != currentPos)
			{
				var distance = Vector3.Distance(currentPos, startPos);

				if (DistanceMin > 0.0f && distance < DistanceMin)
				{
					distance = DistanceMin;
				}

				if (DistanceMax > 0.0f && distance > DistanceMax)
				{
					distance = DistanceMax;
				}

				currentPos = startPos + (currentPos - startPos).normalized * distance;
			}

			return D2dHelper.Atan2(currentPos - startPos) * Mathf.Rad2Deg;
		}

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

				// Throw prefab?
				if (mainCamera != null && ProjectilePrefab != null)
				{
					// Calc values
					var startPos   = D2dHelper.ScreenToWorldPosition( startMousePosition, Intercept, mainCamera);
					var currentPos = D2dHelper.ScreenToWorldPosition(Input.mousePosition, Intercept, mainCamera);
					var angle      = GetAngleAndClampCurrentPos(startPos, ref currentPos) + ProjectileAngle + Random.Range(-ProjectileSpread, ProjectileSpread);

					// Spawn
					var projectile = Instantiate(ProjectilePrefab, startPos, Quaternion.Euler(0.0f, 0.0f, -angle));

					projectile.SetActive(true);

					// Apply velocity?
					var rigidbody2D = projectile.GetComponent<Rigidbody2D>();

					if (rigidbody2D != null)
					{
						rigidbody2D.velocity = (currentPos - startPos) * ProjectileSpeed;
					}
				}
			}

			// Update indicator?
			if (down == true && mainCamera != null && IndicatorPrefab != null)
			{
				if (indicatorInstance == null)
				{
					indicatorInstance = Instantiate(IndicatorPrefab);

					indicatorInstance.gameObject.SetActive(true);
				}

				var startPos   = D2dHelper.ScreenToWorldPosition( startMousePosition, Intercept, mainCamera);
				var currentPos = D2dHelper.ScreenToWorldPosition(Input.mousePosition, Intercept, mainCamera);
				var angle      = GetAngleAndClampCurrentPos(startPos, ref currentPos);
				var scale      = Vector3.Distance(currentPos, startPos) * Scale;

				// Transform the indicator so it lines up with the slice
				indicatorInstance.transform.position   = startPos;
				indicatorInstance.transform.rotation   = Quaternion.Euler(0.0f, 0.0f, -angle);
				indicatorInstance.transform.localScale = new Vector3(scale, scale, scale);
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
	[CustomEditor(typeof(D2dDragToThrow))]
	public class D2dDragToThrow_Editor : D2dEditor<D2dDragToThrow>
	{
		protected override void OnInspector()
		{
			Draw("Requires");
			Draw("Intercept");
			BeginError(Any(t => t.IndicatorPrefab == null));
				Draw("IndicatorPrefab");
			EndError();
			Draw("Scale");
			Draw("DistanceMin");
			Draw("DistanceMax");

			Separator();

			BeginError(Any(t => t.ProjectilePrefab == null));
				Draw("ProjectilePrefab");
			EndError();
			Draw("ProjectileSpeed");
			Draw("ProjectileSpread");
			Draw("ProjectileAngle");
		}
	}
}
#endif