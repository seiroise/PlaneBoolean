using Scripts.RBTree;
using Seiro.Scripts.Geometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntersectionDetector {

	/// <summary>
	/// 走査線ベースの交差検出
	/// </summary>
	public class PlaneSweepIntersectionDetector : IIntersectionDetector {

		/// <summary>
		/// 交差検出
		/// </summary>
		public List<Intersection> Execute(List<LineSegment> segments) {
			
			//イベントキューを作成
			RBTree<Event> eventQueue = new RBTree<Event>();

			foreach (LineSegment s in segments) {
				//線分の端点のうち上にある方を始点，下にある方を終点としてイベントを登録
				//線分が水平な場合は左の端点を始点とする
				if (s.p1.y > s.p2.y || (s.p1.y == s.p2.y && s.p1.x < s.p2.x)) {
					eventQueue.Insert(new Event(Event.Type.SEGMENT_START, s.p1, s, null));
					eventQueue.Insert(new Event(Event.Type.SEGMENT_START, s.p2, s, null));
				} else {
					eventQueue.Insert(new Event(Event.Type.SEGMENT_START, s.p2, s, null));
					eventQueue.Insert(new Event(Event.Type.SEGMENT_START, s.p1, s, null));
				}
			}

			SweepLineBasedComparator sweepComparator = new SweepLineBasedComparator();
			//ステータスを作成。要素の順序関係はsweepComparatorに従う
			RBTree<LineSegment> status = new RBTree<LineSegment>(sweepComparator);

			//今回の実装では同一の交点が複数回検出される可能性があるため、HashSetを使って重複を防ぐ
			HashSet<Intersection> result = new HashSet<Intersection>();

			Event e;
			//キューから先頭のイベントを取り出す
			while ((e = eventQueue.PopSmallest()) != null) {
				float sweepY = e.point.y;
				switch(e.type) {
					case Event.Type.SEGMENT_START:
						//始点イベント
						sweepComparator.SetY(sweepY);

						LineSegment newSegment = e.segment1;
						status.Insert(newSegment);	//ステータスに線分を追加

						LineSegment left = status.

						break;
				}
			}
		}
	}
}