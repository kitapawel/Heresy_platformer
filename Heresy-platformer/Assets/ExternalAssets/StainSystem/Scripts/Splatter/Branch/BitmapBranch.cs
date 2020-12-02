using UnityEngine;

namespace SplatterSystem {
	public class BitmapBranch : BaseBranch {
		private SplatterArea area;

		override public void SetParticleProvider(MonoBehaviour particleProvider) {
			area = (SplatterArea) particleProvider;
		}

        override protected void SpawnParticle(Vector3 position, float scale, Color color) {
			Vector3 planeNormal = area.rectTransform.forward;
			var distance = -Vector3.Dot(planeNormal.normalized, position - area.rectTransform.position);
			Vector3 projectedPosistion = position + planeNormal * distance;
			area.SpawnParticle(settings.particleMode, projectedPosistion, scale, color);
        }
    }
}
