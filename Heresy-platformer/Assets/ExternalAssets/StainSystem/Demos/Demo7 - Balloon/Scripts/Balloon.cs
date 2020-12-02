using System.Collections;
using UnityEngine;

namespace SplatterSystem.Demos {
	
	public class Balloon : MonoBehaviour {
		public RectTransform hitBox;
		public AbstractSplatterManager splatter;
		public Shaker screenShake;

		private Vector3 offset;

		void Start() {
			if (splatter == null) {
				splatter = FindObjectOfType<AbstractSplatterManager>();
			}
		}

		void Update () {
			bool justClicked = Input.GetMouseButtonDown(0);

			if (justClicked) {
				Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if (hitBox.rect.Contains(worldPos - (Vector2)hitBox.transform.position)) {
					StartCoroutine(HandleBaloonPop(worldPos));
				}
			}
		}

		void LateUpdate() {
			transform.position += offset;
		}

		private IEnumerator HandleBaloonPop(Vector3 pos) {
			var renderer = GetComponent<SpriteRenderer>();
			renderer.enabled = false;

			var audio = GetComponent<AudioSource>();
			if (audio != null) {
				audio.Play();
			}

			splatter.Spawn(pos);
			if (screenShake != null) {
				screenShake.Shake();
			}

			yield return new WaitForSeconds(0.5f);
			renderer.enabled = true;
			var color = Color.white;
			color.a = 0f;
			renderer.color = color;
			var wait = new WaitForEndOfFrame();
			while (color.a < 1f) {
				color.a += 5f * Time.deltaTime;
				offset.y = -(1f - color.a) * 1.5f;
				renderer.color = color;
				yield return wait;
			}
			offset = Vector3.zero;

			yield return 0;
		}
	}
}
