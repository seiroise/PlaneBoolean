using UnityEngine;
using Seiro.Scripts.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntersectionDetector {

	/// <summary>
	/// 線分の交差を表現
	/// </summary>
	public class Intersection {

		public LineSegment seg1;
		public LineSegment seg2;

		public Intersection(LineSegment seg1, LineSegment seg2) {
			this.seg1 = seg1;
			this.seg2 = seg2;
		}

		public Vector2 GetIntersectionPoint() {
			Vector2 p = Vector2.zero;
			seg1.GetIntersectionPoint(seg2, ref p);
			return p;
		}

		public override bool Equals(object obj) {
			if (obj == this) {
				return true;
			} else if(obj is Intersection) {
				Intersection other = (Intersection)obj;
				if (seg1.Equals(other.seg1) && seg2.Equals(other.seg2)) {
					return true;
				} else if (seg1.Equals(other.seg2) && seg2.Equals(other.seg1)) {
					return true;
				}
			}
			return false;
		}

		public override int GetHashCode() {
			return seg1.GetHashCode() + seg2.GetHashCode();
		}
	}
}