using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seiro.Scripts.Geometric;

namespace IntersectionDetector {
	
	/// <summary>
	/// 線分交差判定時のイベント
	/// </summary>
	public class Event : IComparable<Event> {

		/// <summary>
		/// イベントの種類
		/// </summary>
		public enum Type {
			SEGMENT_START,	//線分の始点
			SEGMENT_END,	//線分の終点
			INTERSECTION	//線分同士の交差
		}

		public Type type;
		public Vector2 point;
		public LineSegment segment1;	//点に関連する線分1
		public LineSegment segment2;	//点に関連する線分2(type == INTERSECTIONのときのみ)

		public Event(Type type, Vector2 point, LineSegment segment1, LineSegment segment2 = null) {
			this.type = type;
			this.point = point;
			this.segment1 = segment1;
			this.segment2 = segment2;
		}

		public int CompareTo(Event other) {

			//y座標値の比較
			int com = point.y.CompareTo(other.point.y);

			//y座標が等しい場合はx座標を比較
			if (com == 0) {
				com = point.x.CompareTo(other.point.x);
			}
			return com;
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("(");
			sb.Append(point.x);
			sb.Append(" , ");
			sb.Append(point.y);
			sb.Append(")");
			return sb.ToString();
		}
	}
}