using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seiro.Scripts.Geometric;

namespace PlaneBoolean1 {

	/// <summary>
	/// 多角形の辺
	/// </summary>
	public class PolygonEdge2D {

		public readonly Vector2 from;
		public readonly Vector2 to;
		public readonly Vector2 delta;

		public readonly LineSegment lineSeg;

		#region Constructor

		public PolygonEdge2D(Vector2 from, Vector2 to) {
			this.from = from;
			this.to = to;
			this.delta = to - from;

			this.lineSeg = new LineSegment(from, to);
		}

		#endregion
	}
}