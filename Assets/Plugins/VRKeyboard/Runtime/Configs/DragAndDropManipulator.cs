using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace VRKeyboard.Runtime
{
	public class DragAndDropManipulator : PointerManipulator
	{
        public DragAndDropManipulator(VisualElement target, VisualElement root)
        {
            this.target = target;
            //root = target.parent;
            this.root = root;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
            target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
            target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
        }

        private Vector2 targetStartPosition { get; set; }

        private Vector3 pointerStartPosition { get; set; }

        private bool enabled { get; set; }

        private VisualElement root { get; }

        private void PointerDownHandler(PointerDownEvent evt)
        {
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
            }
        }

        private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
        {
            if (enabled)
            {
                Slots slotsContainer = root.Q<Slots>();
                UQueryBuilder<Slot> allSlots = slotsContainer.Query<Slot>();
                UQueryBuilder<Slot> overlappingSlots = allSlots.Where(OverlapsTarget);
                Slot closestOverlappingSlot = FindClosestSlot(overlappingSlots);
                Vector3 closestPos = Vector3.zero;
                if (closestOverlappingSlot != null)
                {
                    closestPos = RootSpaceOfSlot(closestOverlappingSlot);
                    closestPos = new Vector2(closestPos.x - 5, closestPos.y - 5);
                }

                if (closestOverlappingSlot != null)
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

        private bool OverlapsTarget(Slot slot)
        {
            return target.worldBound.Overlaps(slot.worldBound);
        }

        private Slot FindClosestSlot(UQueryBuilder<Slot> slots)
        {
            List<Slot> slotsList = slots.ToList();
            float bestDistanceSq = float.MaxValue;
            Slot closest = null;
            foreach (Slot slot in slotsList)
            {
                Vector3 displacement = RootSpaceOfSlot(slot) - target.transform.position;
                float distanceSq = displacement.sqrMagnitude;
                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    closest = slot;
                }
            }
            return closest;
        }

        private Vector3 RootSpaceOfSlot(Slot slot)
        {
            Vector2 slotWorldSpace = slot.parent.LocalToWorld(slot.layout.position);
            return root.WorldToLocal(slotWorldSpace);
        }
    }
}
