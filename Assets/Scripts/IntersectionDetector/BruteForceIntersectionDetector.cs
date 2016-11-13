using Seiro.Scripts.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntersectionDetector {

	/// <summary>
	/// 総当り交差検出
	/// </summary>
	public class BruteForceIntersectionDetector : IIntersectionDetector {

		public List<Intersection> Execute(List<LineSegment> segments) {
			List<Intersection> result = new List<Intersection>();
			int size = segments.Count;
			for(int i = 0; i < size; ++i) {
				LineSegment s1 = segments[i];
				for (int j = i + 1; j < size; ++j) {
					LineSegment s2 = segments[j];
					if (s1.Intersects(s2)) {
						result.Add(new Intersection(s1, s2));
					}
				}
			}
			return result;
		}
	}
}