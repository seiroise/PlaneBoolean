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
		public float area = 10f;
		private PlaneSweepIntersectionDetector detector;
		private ChainLineFactory lineFactory;
		private List<Intersection> intersections;

		private ChainLine sweepLine;
		private List<ChainLine> statusLine;
		private List<ChainLine> intersectionLine;

		#region UnityEvent

		private void Start() {
			this.detector = new PlaneSweepIntersectionDetector();
			this.lineFactory = GetComponent<ChainLineFactory>();
			this.intersections = new List<Intersection>();

			statusLine = new List<ChainLine>();

			Test();
		}

		#endregion

		#region Function

		/// <summary>
		/// テスト
		/// </summary>
		private void Test() {

			List<LineSegment> segments = new List<LineSegment>();

			for(int i = 0; i < sample; ++i) {
				Vector2 p1, p2;
				p1 = new Vector2(Random.Range(-area, area), Random.Range(-area, area));
				p2 = new Vector2(Random.Range(-area, area), Random.Range(-area, area));
				segments.Add(new LineSegment(p1, p2));

				List<Vector3> vertices = new List<Vector3>();
				vertices.Add(p1);
				vertices.Add(p2);
				lineFactory.CreateLine(vertices, Color.blue);
			}

			//交差判定
			StartCoroutine(detector.CoDebug(segments, PopEvent, Status, AddIntersection));
		}

		#endregion

		#region Callback

		private void PopEvent(IntersectionDetector.Event e) {
			lineFactory.DeleteLine(sweepLine);

			List<Vector3> vertices = new List<Vector3>();
			vertices.Add(new Vector3(-area, e.point.y));
			vertices.Add(new Vector3(area, e.point.y));

			sweepLine = lineFactory.CreateLine(vertices, Color.red);
		}

		private void Status(RBTree<LineSegment> status) {
			foreach (var e in statusLine) {
				lineFactory.DeleteLine(e);
			}
			statusLine = new List<ChainLine>();

			Debug.Log(status.Count + ": " + status);
			foreach (var l in status) {
				Debug.Log(l);
				List<Vector3> vertices = new List<Vector3>();
				vertices.Add(l.p1);
				vertices.Add(l.p2);
				statusLine.Add(lineFactory.CreateLine(vertices, Color.yellow));
			}
		}

		private void AddIntersection(Vector2 point) {
			List<Vector3> vertices = new List<Vector3>();
			vertices.Add(point + Vector2.right);
			vertices.Add(point + Vector2.down);
			vertices.Add(point + Vector2.left);
			vertices.Add(point + Vector2.up);
			vertices.Add(point + Vector2.right);
			lineFactory.CreateLine(vertices, Color.magenta);
		}

		#endregion
	}
}