using UnityEngine;

namespace SplatterSystem {
	#pragma warning disable 0414

	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class SplatterArea : MonoBehaviour {
		public float pixelsPerUnit = 100f;
		public float scaleMultiplier = 1f;
		public bool debugDraw = false;

		public RectTransform rectTransform { get; private set; }

		private SpriteRenderer spriteRenderer;
		private Sprite sprite;
		public Texture2D texture;
		private bool isResizing = false;
		private bool isDirty = false;

		private Vector3 _cacheVec3;
		private Vector2 _cacheVec2;

		void OnEnable() {
			rectTransform = GetComponent<RectTransform>();
			spriteRenderer = GetComponent<SpriteRenderer>();

			if (Application.isPlaying) {
				debugDraw = false;
			}
			
			sprite = spriteRenderer.sprite;
			if (sprite == null) {
				ResetCanvasTexture();
				sprite = spriteRenderer.sprite;
			}
			texture = sprite.texture;
		}

		void LateUpdate() {
			if (isDirty) {
				texture.Apply();
				isDirty = false;
			}

			// Handle resizing.
	#if UNITY_EDITOR
			if (!Application.isPlaying && isResizing) {
				if (Input.GetMouseButton(0)) {
					ResetSprite();
				}
				if (!Input.GetMouseButton(0)) {
					ResetCanvasTexture();
					isResizing = false;
				}
			}
	#endif
		}

		void OnValidate() {
	#if !UNITY_2018_1_OR_NEWER
			ResetCanvasTexture();
	#endif
		}

		void OnRectTransformDimensionsChange() {
	#if UNITY_EDITOR
			isResizing = true;
			ResetSprite();
	#endif
		}

		public virtual Texture2D GetTexture() {
			return texture;
		}

		public virtual void SpawnParticle(ParticleMode shape, Vector3 point, float scale, Color color) {
			Vector3 uv = WorldToTexturePosition(ref point);
			int particleSize = (int) (scaleMultiplier * scale * 40f);
			if (shape == ParticleMode.Square) {
				BitmapDraw.DrawFilledSquare(texture, (int)uv.x, (int)uv.y, particleSize, color);
			} else {
				BitmapDraw.DrawFilledCircle(texture, (int)uv.x, (int)uv.y, particleSize, color);
			}		
			isDirty = true;
		}

		public virtual void Apply() {
			isDirty = true;
		}

		public void Clear() {
			ResetCanvasTexture();
		}
		
		public void ResetCanvasTexture() {
			if (!rectTransform || 
				rectTransform.rect.width <= 0 || rectTransform.rect.height <= 0) return;

			texture = new Texture2D((int)(rectTransform.rect.width * pixelsPerUnit), 
									(int)(rectTransform.rect.height * pixelsPerUnit), 
									TextureFormat.ARGB32, 
									false);
			texture.wrapMode = TextureWrapMode.Clamp;

			// Set alpha of all pixels to a minimum non-zero value to work around Unity not showing texture otherwise.
			var pixels = texture.GetPixels32();
			for (int i = 0; i < pixels.Length; i++) {
				pixels[i].a = (byte)(debugDraw? 100 : 1);
			}
			texture.SetPixels32(pixels);
			texture.Apply();

			var rect = new Rect(0, 0, texture.width, texture.height);
			sprite = Sprite.Create(texture, rect, rectTransform.pivot, pixelsPerUnit);
			spriteRenderer.sprite = sprite;
		}

		public Vector2 WorldToTexturePosition(ref Vector3 worldPos) {
			_cacheVec3 = worldPos - rectTransform.position;
			_cacheVec2.Set(Vector3.Dot(_cacheVec3, rectTransform.right), Vector3.Dot(_cacheVec3, rectTransform.up));
			return _cacheVec2 * sprite.pixelsPerUnit + sprite.pivot;
			//return (worldPos - (Vector2)rectTransform.position) * sprite.pixelsPerUnit + sprite.pivot;
		}

		private void ResetSprite() {
			if (spriteRenderer != null) {
				spriteRenderer.sprite = null;
			}
		}

	}

}
