using Seiro.Scripts.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaneBoolean1 {

	/// <summary>
	/// 交差検出インタフェース
	/// </summary>
	public interface IntersectionDetector {

		List<Intersection> Execute(List<LineSegment> segments);
	}
}