using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaneBoolean1 {

	/// <summary>
	/// 二次元多角形
	/// </summary>
	public class Polygon2D {

		public readonly Vector2[] vertices;
		public readonly PolygonEdge2D[] edges;

		#region Constructor

		public Polygon2D(Vector2[] vertices) {

			if (vertices == null || vertices.Length < 1) {
				new ArgumentException("Arugment vertices is not suitable");
			}

			//回転方向の確認
			if (Polygon2D.CheckCW(vertices)) {
				Array.Reverse(vertices);
			}

			//頂点配列の設定
			this.vertices = vertices;

			//頂点配列から辺配列の作成
			this.edges = CreateEdges(vertices);
		}

		#endregion

		#region StaticFunction

		/// <summary>
		/// 頂点配列が時計回り(Clock wise)かを判定する
		/// </summary>
		private static bool CheckCW(Vector2[] vertices) {
			
			//角度の総和を求める
			int len = vertices.Length;
			float sumTheta = 0f;
			for (int i = 0; i < len; ++i) {
				
				//3点の角度を求める
				Vector2 p1, p2, p3;
				p1 = vertices[i];
				p2 = vertices[(i + 1) % len];
				p3 = vertices[(i + 2) % len];
				float thetaA = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x);
				float thetaB = Mathf.Atan2(p3.y - p2.y, p3.x - p2.x);
				float theta = thetaB - thetaA;

				//θが-π~πの範囲を超えないように調整
				if (theta > Mathf.PI) theta -= Mathf.PI;
				else if (theta < -Mathf.PI) theta += Mathf.PI;

				sumTheta += theta;
			}

			//回りの判定
			if (sumTheta < -Mathf.PI + 0.0001f) {
				return true;
			} else if (sumTheta > -Mathf.PI - 0.0001f) {
				return false;
			}

			//どちらでもない(エラー)
			throw new ArgumentException("Argument vertice is error");
		}

		/// <summary>
		/// 頂点配列から辺の作成
		/// </summary>
		private static PolygonEdge2D[] CreateEdges(Vector2[] vertices) {
			
			int len = vertices.Length;
			PolygonEdge2D[] edges = new PolygonEdge2D[len];

			for (int i = 0; i < len; ++i) {
				edges[i] = new PolygonEdge2D(vertices[i], vertices[(i + 1) % len]);
			}
			return edges;
		}

		#endregion

		#region PrivateFunction



		#endregion
	}
}