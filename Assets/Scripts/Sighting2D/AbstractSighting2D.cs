using UnityEngine;
using System.Collections;

/// <summary>
/// 2次元でのサイティングの抽象クラス
/// </summary>
public abstract class AbstractSighting2D : MonoBehaviour {

	public float offset = 0f;
	public float lookSpeed = 10f;
	public UpdateType updateType = UpdateType.Normal;

	#region UnityEvent

	private void Update() {
		if (updateType == UpdateType.Normal) UpdateSighting();
	}

	private void LateUpdate() {
		if (updateType == UpdateType.Late) UpdateSighting();
	}

	private void FixedUpdate() {
		if (updateType == UpdateType.Fixed) UpdateSighting();
	}

	#endregion

	#region VirtualFunction

	/// <summary>
	/// 狙いの更新
	/// </summary>
	protected abstract void UpdateSighting();

	#endregion

	#region Function

	/// <summary>
	/// 狙いの更新
	/// </summary>
	protected void UpdateSighting(Vector2 sightPoint) {
		//目標角度
		float tAngle = GeomUtil.TwoPointAngle(transform.position, sightPoint);
		transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(transform.eulerAngles.z, tAngle, lookSpeed * Time.deltaTime));
	}

	#endregion
}