using UnityEngine;
using System.Collections;

/// <summary>
/// 弾
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

	private Rigidbody2D rBody2d;
	public float speed = 100f;

	#region UnityEvent

	private void Start() {
		
		if (!rBody2d) rBody2d = GetComponent<Rigidbody2D>();

		if (rBody2d) {
			rBody2d.velocity = transform.right * speed;
		}
	}

	private void OnCollisionEnter2D(Collision2D co) {
		Destroy(gameObject);
	}

	#endregion
}