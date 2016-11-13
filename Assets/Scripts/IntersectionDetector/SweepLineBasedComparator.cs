using UnityEngine;
using Seiro.Scripts.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntersectionDetector {
	
	/// <summary>
	/// ステータスの更新を行う走査線
	/// </summary>
	public class SweepLineBasedComparator : IComparer<LineSegment>{

		private Line sweepLine;
		private Line berowLine;

		public SweepLineBasedComparator() {
			SetY(0f);
		}

		public SweepLineBasedComparator(float y) {
			SetY(y);
		}

		/// <summary>
		/// 走査線のy座標を更新
		/// </summary>
		public void SetY(float y) {
			//走査線を更新
			sweepLine = Line.FromPoints(0, y, 1, y);
			//走査線の少し下を通る線を作成
			berowLine = Line.FromPoints(0, y - 0.01f, 1, y - 0.01f);
		}

		/// <summary>
		/// IComparerの実装
		/// </summary>
		public int Compare(LineSegment s1, LineSegment s2) {
			int com = CompareByLine(s1, s2, sweepLine);
			if (com == 0) {
				//走査線上の交点が等しい場合は、走査線の少し下の位置で比較
				com = CompareByLine(s1, s2, berowLine);
			}
			return com;
		}

		/// <summary>
		/// s1とs2をlineとの交点のx座標にもとづいて比較
		/// </summary>
		private int CompareByLine(LineSegment s1, LineSegment s2, Line line) {
			Vector2 p1, p2;
			float x1 = s1.GetIntersectionPoint(line, out p1) ? p1.x : s1.p1.x;
			float x2 = s2.GetIntersectionPoint(line, out p2) ? p2.x : s2.p1.x;
			return x1.CompareTo(x2);
		}
	}
}