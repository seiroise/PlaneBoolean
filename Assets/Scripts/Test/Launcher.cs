using UnityEngine;
using System.Collections;

/// <summary>
/// 砲台
/// </summary>
public class Launcher : MonoBehaviour {

	public Bullet bulletPrefab;

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Bullet b = Instantiate<Bullet>(bulletPrefab);
			b.transform.position = transform.position;
			b.transform.rotation = transform.rotation;
		}
	}
}