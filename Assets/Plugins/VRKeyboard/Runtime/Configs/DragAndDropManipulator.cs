using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace VRKeyboard.Runtime
{
	public class DragAndDropManipulator : PointerManipulator
	{
        public DragAndDropManipulator(List<VisualElement> targets, VisualElement root)
        {
            this.allTargets = targets;
            this.root = root;

            this.target = new VisualElement();
        }

        public void AddTarget()
		{

		}

        public void RemoveOrClearTargets()
		{

		}

        protected override void RegisterCallbacksOnTarget()
        {
            allTargets.ForEach(x => x.RegisterCallback<PointerDownEvent>(PointerDownHandler));
            allTargets.ForEach(x => x.RegisterCallback<PointerMoveEvent>(PointerMoveHandler));
            allTargets.ForEach(x => x.RegisterCallback<PointerUpEvent>(PointerUpHandler));
            allTargets.ForEach(x => x.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler));
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            allTargets.ForEach(x => x.UnregisterCallback<PointerDownEvent>(PointerDownHandler));
            allTargets.ForEach(x => x.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler));
            allTargets.ForEach(x => x.UnregisterCallback<PointerUpEvent>(PointerUpHandler));
            allTargets.ForEach(x => x.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler));
        }

        private List<VisualElement> allTargets = default;

        private Vector2 targetStartPosition { get; set; }

        private Vector3 pointerStartPosition { get; set; }

        private bool enabled { get; set; }

        private VisualElement root { get; }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            target = (VisualElement)evt.target;

            targetStartPosition = target.transform.position;
            pointerStartPosition = evt.position;
            target.CapturePointer(evt.pointerId);
            enabled = true;
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            if (enabled && target.HasPointerCapture(evt.pointerId))
            {
                Vector3 pointerDelta = evt.position - pointerStartPosition;

                target.transform.position = new Vector2(
                    Mathf.Clamp(targetStartPosition.x + pointerDelta.x, 0, target.panel.visualTree.worldBound.width),
                    Mathf.Clamp(targetStartPosition.y + pointerDelta.y, 0, target.panel.visualTree.worldBound.height));
            }
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            if (enabled && target.HasPointerCapture(evt.pointerId))
            {
                target.ReleasePointer(evt.pointerId);
                target = new VisualElement();
            }
        }

        private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
        {
            if (enabled)
            {
                Container rowsContainer = root.Q<Container>();
                UQueryBuilder<Row> allRows = rowsContainer.Query<Row>();
                UQueryBuilder<Row> overlappingRows = allRows.Where(OverlapsTarget);
                Row closestOverlappingRow = FindClosestRow(overlappingRows);
                Vector3 closestPos = Vector3.zero;
                if (closestOverlappingRow != null)
                {
                    closestPos = RootSpaceOfRow(closestOverlappingRow);
                    closestPos = new Vector2(closestPos.x - 5, closestPos.y - 5);
                }

                if (closestOverlappingRow != null)
				{
                    target.transform.position = closestPos;
                }
                else
				{
                    target.transform.position = targetStartPosition;
                }

                enabled = false;
            }
        }

        private bool OverlapsTarget(Row row)
        {
            return target.worldBound.Overlaps(row.worldBound);
        }

        private Row FindClosestRow(UQueryBuilder<Row> rows)
        {
            List<Row> rowsList = rows.ToList();
            float bestDistanceSq = float.MaxValue;
            Row closest = null;
            foreach (Row row in rowsList)
            {
                Vector3 displacement = RootSpaceOfRow(row) - target.transform.position;
                float distanceSq = displacement.sqrMagnitude;
                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    closest = row;
                }
            }
            return closest;
        }

        private Vector3 RootSpaceOfRow(Row row)
        {
            Vector2 rowWorldSpace = row.parent.LocalToWorld(row.layout.position);
            return root.WorldToLocal(rowWorldSpace);
        }
    }
}
