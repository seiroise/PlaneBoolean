using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public Bullet bulletPrefab;

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Bullet b = Instantiate<Bullet>(bulletPrefab);
			b.transform.position = transform.position;
			b.transform.rotation = transform.rotation;
		}
	}
}