using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Seiro.Scripts.Geometric;
using IntersectionDetector;
using Seiro.Scripts.Graphics.ChainLine;
using Seiro.Scripts.Utility;
using Scripts.RBTree;

namespace IntersectionTest {

	/// <summary>
	/// 走査線ベースの交差判定のテスト
	/// </summary>
	[RequireComponent(typeof(ChainLineFactory))]
	public class SweepLineIntersectionTest : MonoBehaviour {

		public int sample = 100;
		private PlaneSweepIntersectionDetector detector;
		private ChainLineFactory lineFactory;
		private List<Intersection> intersections;

		#region UnityEvent

		private void Start() {
			this.detector = new PlaneSweepIntersectionDetector();
			this.lineFactory = GetComponent<ChainLineFactory>();
			this.intersections = new List<Intersection>();
			Test();
		}

		#endregion

		#region Function

		/// <summary>
		/// テスト
		/// </summary>
		private void Test() {

			List<LineSegment> segments = new List<LineSegment>();
			float area = 5f;

			for(int i = 0; i < sample; ++i) {
				Vector2 p1, p2;
				p1 = new Vector2(Random.Range(-area, area), Random.Range(-area, area));
				p2 = new Vector2(Random.Range(-area, area), Random.Range(-area, area));
				Debug.Log(p1 + " - " + p2);
				segments.Add(new LineSegment(p1, p2));
			}

			//交差判定
			intersections = detector.Execute(segments);

			//線分の描画
			for(int i = 0; i < segments.Count; ++i) {
				List<Vector3> vertices = new List<Vector3>();
				vertices.Add(segments[i].p1);
				vertices.Add(segments[i].p2);
				lineFactory.CreateLine(vertices, Color.blue);
			}

			//交点の描画
			for(int i = 0; i < intersections.Count; ++i) {
				List<Vector3> vertices = new List<Vector3>();
				Vector3 p = intersections[i].GetIntersectionPoint();
				vertices.Add(p + Vector3.up * 0.1f);
				vertices.Add(p + Vector3.left * 0.1f);
				vertices.Add(p + Vector3.down * 0.1f);
				vertices.Add(p + Vector3.right * 0.1f);
				vertices.Add(p + Vector3.up * 0.1f);
				lineFactory.CreateLine(vertices, Color.red);
			}
		}

		#endregion
	}
}