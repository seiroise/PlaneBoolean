using UnityEngine;
using System.Collections;

/// <summary>
/// 二次元剛体の操作
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyController2D : MonoBehaviour {

	private Rigidbody2D rBody2D;

	[Header("Speed")]
	[SerializeField, Range(1f, 1000f)]
	private float maxSpeed = 10f;		//最高速度
	[SerializeField, Range(1f, 1000f)]
	private float acceleration = 10f;	//加速力

	[Header("Brake")]
	[SerializeField, Range(0f, 100f)]
	private float braking = 1f;			//ブレーーーキ力

	[Header("Steering")]
	[SerializeField, Range(1f, 1000f)]
	private float steering = 1f;		//旋回性能
	[SerializeField, Range(0f, 10f)]
	private float steeringAssist = 1f;	//補助旋回性能(船でいうバウスラスターの強さ)

	[Header("Option")]
	[SerializeField]
	private bool indicateDebug = false;	//デバッグ情報の表示

	#region UnityEvent

	private void Awake() {
		rBody2D = GetComponent<Rigidbody2D>();
	}

	private void OnGUI() {
		if (!indicateDebug) return;
		GUILayout.Label("NowSpeed : " + rBody2D.velocity.magnitude);
	}

	#endregion

	#region Function

	/// <summary>
	/// 速度ベクトルを設定する
	/// </summary>
	public void SetVelocity(Vector2 velocity) {
		Vector2 nowVelocity = rBody2D.velocity;						//現在速度ベクトル
		Vector2 delta = velocity - nowVelocity;						//差分ベクトル
		Vector2 adjust = (nowVelocity - delta) * steeringAssist;	//調整ベクトル
		rBody2D.AddForce(delta - adjust);
	}

	/// <summary>
	/// 前方にspeedだけ加速する
	/// </summary>
	public void Accele(float speed) {
		SetVelocity(transform.right * speed);
	}

	/// <summary>
	/// 前方に最高速まで加速する
	/// </summary>
	public void Accele() {
		Accele(maxSpeed);
	}

	/// <summary>
	/// 指定した方向に最高速まで加速する
	/// </summary>
	public void Accele(Vector2 dir) {
		SetVelocity(dir * maxSpeed);
	}

	/// <summary>
	/// ブレーキをかける
	/// </summary>
	public void Brake() {
		if (braking == 0f) return;
		float speed = rBody2D.velocity.magnitude;
		float b = speed > braking ? braking : speed;
		Accele(-b);
	}

	/// <summary>
	/// 角速度を設定する
	/// </summary>
	public void SetAngulerVelocity(float angVelo) {
		float delta = angVelo - rBody2D.angularVelocity;
		rBody2D.AddTorque(delta);
	}

	/// <summary>
	/// 回転する
	/// </summary>
	public void Rotation(float angle) {
		float delta = Mathf.DeltaAngle(transform.eulerAngles.z, angle) / 180f;
		SetAngulerVelocity(delta * steering);
	}
	#endregion
}