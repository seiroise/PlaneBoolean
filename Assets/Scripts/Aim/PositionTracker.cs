using UnityEngine;
using System.Collections;

/// <summary>
/// 座標の追跡
/// </summary>
public class PositionTracker : MonoBehaviour {

	public Transform aimTrans;
	public Vector3 offset;
	public UpdateType updateType = UpdateType.Normal;
	public bool[] constrains = new bool[3];

	#region UnityEvent

	private void Update() {
		if (updateType == UpdateType.Normal) UpdatePosition();
	}

	private void LateUpdate() {
		if (updateType == UpdateType.Late) UpdatePosition();
	}

	private void FixedUpdate() {
		if (updateType == UpdateType.Fixed) UpdatePosition();
	}

	#endregion

	#region Function

	/// <summary>
	/// 座標の追跡
	/// </summary>
	private void UpdatePosition() {
		
		if (!aimTrans) return;

		//目標座標
		Vector3 tPos = transform.position;
		for (int i = 0; i < constrains.Length; ++i) {
			if (constrains[i]) continue;
			tPos[i] = aimTrans.position[i] + offset[i];
		}
		transform.position = tPos;
	}

	#endregion
}