using UnityEngine;
using System.Collections;

namespace Seiro.Scripts.Move2D.Rigidbody {

	/// <summary>
	/// 物理エンジンを利用したそれっぽい操作
	/// </summary>
	[RequireComponent(typeof(RigidbodyController2D))]
	public class MoveController : MonoBehaviour {

		private RigidbodyController2D rController2D;
		[SerializeField]
		private bool turnRotated = true;

		#region UnityEvent

		private void Awake() {
			rController2D = GetComponent<RigidbodyController2D>();
		}

		private void Update() {
			CheckInput();
		}

		#endregion

		#region Function

		/// <summary>
		/// 入力確認
		/// </summary>
		private void CheckInput() {
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			if (h == 0 && v == 0) {
				rController2D.Brake();
			} else {
				float angle = GetOriginAim(h, v);
				rController2D.Rotation(angle);
				if (turnRotated) {
					rController2D.Accele();
				} else {
					rController2D.Accele(new Vector2(h, v));
				}
			}
		}

		/// <summary>
		/// 原点からのベクトルの角度を求める
		/// </summary>
		public float GetOriginAim(float x, float y) {
			if (x == 0f && y == 0f) return 0f;
			float rad = Mathf.Atan2(y, x);
			//数値を補正
			return rad * Mathf.Rad2Deg;
		}

		/// <summary>
		/// 原点からのベクトルの角度を求める
		/// </summary>
		public float GetAim(Vector2 p) {
			return GetOriginAim(p.x, p.y);
		}

		/// <summary>
		/// ベクトルp1からp2への角度を求める
		/// </summary>
		public float GetAim(Vector2 p1, Vector2 p2) {
			float dx = p2.x - p1.x;
			float dy = p2.y - p1.y;
			return GetOriginAim(dx, dy);
		}

		#endregion
	}
}