using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seiro.Scripts.Graphics.ChainLine;
using IntersectionDetector;
using Seiro.Scripts.Geometric;
using Scripts.RBTree;

namespace IntersectionTest {

	/// <summary>
	/// 走査線ベースのこさ判定の可視化
	/// </summary>
	[RequireComponent(typeof(ChainLineFactory))]
	public class VisualizeTest : MonoBehaviour {

		public int sample = 100;
		public float area = 10f;
		[Range(0.01f, 10f)]
		public float markerSize = 1f;
		public Color intersectionColor;
		public Color segmentColor;
		public Color activeColor;

		private PlaneSweepIntersectionDetector planeSweep;
		private BruteForceIntersectionDetector bruteForce;
		private ChainLineFactory lineFactory;
		private List<Intersection> intersections;

		private ChainLine sweepLine;
		private List<ChainLine> statusLine;
		private List<ChainLine> intersectionLine;


		#region UnityEvent

		private void Start() {
			this.planeSweep = new PlaneSweepIntersectionDetector();
			this.bruteForce = new BruteForceIntersectionDetector();
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

			for (int i = 0; i < sample; ++i) {
				Vector2 p1, p2;
				p1 = new Vector2(Random.Range(-area, area), Random.Range(-area, area));
				p2 = new Vector2(Random.Range(-area, area), Random.Range(-area, area));
				segments.Add(new LineSegment(p1, p2));
			}

			DrawSegments(segments, Vector2.zero);

			//デバッグ用
			StartCoroutine(planeSweep.CoDebug(segments, PopEvent, Status, AddIntersection));
		}

		/// <summary>
		/// 線分の描画
		/// </summary>
		private void DrawSegments(List<LineSegment> segments, Vector2 offset) {
			foreach (var e in segments) {
				List<Vector3> vertices = new List<Vector3>();
				vertices.Add(e.p1 + offset);
				vertices.Add(e.p2 + offset);
				lineFactory.CreateLine(vertices, segmentColor);
			}
		}

		/// <summary>
		/// 交点の描画
		/// </summary>
		private void DrawIntersection(Vector2 p) {
			List<Vector3> vertices = new List<Vector3>();
			vertices.Add(p + Vector2.right * markerSize);
			vertices.Add(p + Vector2.down * markerSize);
			vertices.Add(p + Vector2.left * markerSize);
			vertices.Add(p + Vector2.up * markerSize);
			vertices.Add(vertices[0]);
			lineFactory.CreateLine(vertices, intersectionColor);
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

			foreach (var l in status) {
				Debug.Log(l);
				List<Vector3> vertices = new List<Vector3>();
				vertices.Add(l.p1);
				vertices.Add(l.p2);
				statusLine.Add(lineFactory.CreateLine(vertices, activeColor));
			}
		}

		private void AddIntersection(Vector2 point) {
			DrawIntersection(point);
		}

		#endregion
	}
}