using Seiro.Scripts.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntersectionDetector {

	/// <summary>
	/// 交差検出インタフェース
	/// </summary>
	public interface IIntersectionDetector {

		List<Intersection> Execute(List<LineSegment> segments);
	}
}