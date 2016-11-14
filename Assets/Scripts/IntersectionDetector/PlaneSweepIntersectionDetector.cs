using UnityEngine;
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

			foreach(LineSegment s in segments) {
				//線分の端点のうち上にある方を始点，下にある方を終点としてイベントを登録
				//線分が水平な場合は左の端点を始点とする
				if(s.p1.y > s.p2.y || (s.p1.y == s.p2.y && s.p1.x < s.p2.x)) {
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
			while((e = eventQueue.PopSmallest()) != null) {
				float sweepY = e.point.y;
				switch(e.type) {
				case Event.Type.SEGMENT_START:
					//始点イベントの場合
					sweepComparator.SetY(sweepY);   //走査線の更新

					LineSegment newSegment = e.segment1;
					status.Insert(newSegment);  //ステータスに線分を追加

					LineSegment left = status.GetSmaller(newSegment);
					LineSegment right = status.GetBigger(newSegment);

					//左隣の線分との交差を調べる
					CheckIntersection(left, newSegment, sweepY, eventQueue);
					//右隣の線分との交差を調べる
					CheckIntersection(newSegment, right, sweepY, eventQueue);
					break;
				case Event.Type.INTERSECTION:
					//交点イベントの場合
					left = e.segment1;
					right = e.segment2;

					//交点を結果に追加
					result.Add(new Intersection(left, right));

					LineSegment moreLeft = status.GetSmaller(left);
					LineSegment moreRight = status.GetSmaller(right);

					//ステータス中のleftとrightの位置を交換するためいったん削除する
					status.Delete(left);
					status.Delete(right);

					sweepComparator.SetY(sweepY);   //走査線の更新

					//計算誤差により、走査線の更新後も順序が交換されない場合は
					//走査線を少し下げて順序が確実に変わるようにする
					if(sweepComparator.Compare(left, right) < 0) {
						sweepComparator.SetY(sweepY - 0.001f);
					}
					//更新後の走査線を基準としてleftとrightを再追加(位置が交換される)
					status.Insert(left);
					status.Insert(right);

					//right(位置交換によって新しく左側に来た線分)と、そのさらに左隣の線分の交差を調べる
					CheckIntersection(moreLeft, right, sweepY, eventQueue);
					//left(位置交換によって新しく右側に来た線分)と、そのさらに右隣の線分の交差を調べる
					CheckIntersection(left, moreRight, sweepY, eventQueue);
					break;
				case Event.Type.SEGMENT_END:
					//終点イベントの場合
					LineSegment endSegment = e.segment1;
					left = status.GetSmaller(endSegment);
					right = status.GetBigger(endSegment);

					//線分の削除によって新しく隣り合う2線分の交差を調べる
					CheckIntersection(left, right, sweepY, eventQueue);
					status.Delete(endSegment);	//ステータスから線分を削除

					sweepComparator.SetY(sweepY);	//走査線を更新
					break;
				}
			}
			return result.ToList();
		}

		/// <summary>
		/// 線分leftとrightが走査線の下で交差するかどうか調べ、交差する場合は交点イベントを登録する
		/// </summary>
		private void CheckIntersection(LineSegment left, LineSegment right, float sweepY, RBTree<Event> eventQueue) {

			//2線分のうち少なくとも一方が存在しない場合、何もしない
			if(left == null || right == null) return;

			//交点を求める
			Vector2 p;
			if(!left.GetIntersectionPoint(right, out p)) return;

			//交点が走査線よりも下に存在するときのみ、キューに交点イベントを登録
			if(p.y <= sweepY) {
				eventQueue.Insert(new Event(Event.Type.INTERSECTION, p, left, right));
			}
		}
	}
}