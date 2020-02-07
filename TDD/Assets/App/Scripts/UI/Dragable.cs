using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace App
{
    public class Dragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public class DragEvent : UnityEvent<PointerEventData> { }

        private DragEvent onBeginDrag = new DragEvent();
        private DragEvent onDrag = new DragEvent();
        private DragEvent onEndDrag = new DragEvent();

        public DragEvent OnBeginDragEvent { get { return onBeginDrag; } }
        public DragEvent OnDragEvent { get { return onDrag; } }
        public DragEvent OnEndDragEvent { get { return onEndDrag; } }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDrag.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag.Invoke(eventData);
        }
    }
}