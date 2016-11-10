using UnityEngine;
using System.Collections;
using Seiro.Scripts.Utility;

/// <summary>
/// 2次元でのポインターへのサイティング
/// </summary>
public class PointerSighting2D : AbstractSighting2D {

	#region VirtualFunction

	protected override void UpdateSighting() {
		Vector3 mPos = FuncBox.GetMousePosition();
		UpdateSighting(mPos);
	}

	#endregion
}