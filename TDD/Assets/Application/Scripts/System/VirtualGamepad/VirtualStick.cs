using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

namespace App
{
    /// <summary>
    /// 仮想ジョイスティックのスティック
    /// </summary>
    public class VirtualStick : MonoBehaviour
    {
        /// <summary>
        /// タッチ検知エリア
        /// </summary>
        [SerializeField]
        private Dragable raycastTarget = null;

        /// <summary>
        /// スティック画像
        /// </summary>
        [SerializeField]
        private Image stickImage = null;

        /// <summary>
        /// スティック移動範囲
        /// </summary>
        [SerializeField]
        private float stickRange = 32.0f;

        /// <summary>
        /// スティックベクトル
        /// </summary>
        private Vector2 stickVector;

        /// <summary>
        /// 正規化したスティックベクトル
        /// </summary>
        private Vector2 normalizedStickVector;

        /// <summary>
        /// 自身のRectTransform
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// タッチ検知エリアのRectTransform
        /// </summary>
        private RectTransform raycastTargetRectTransform;

        /// <summary>
        /// スティック画像のRectTransform
        /// </summary>
        private RectTransform stickImageRectTransform;

        /// <summary>
        /// スティックの初期位置
        /// </summary>
        private Vector3 defaultPos;

        /// <summary>
        /// UIカメラプロパティ
        /// </summary>
        /// <value></value>
        public Camera UiCamera { get; set; }

        /// <summary>
        /// 横方向の移動量
        /// </summary>
        /// <returns></returns>
        public float GetHolizontal()
        {
            return normalizedStickVector.x;
        }

        /// <summary>
        /// 縦方向の移動量
        /// </summary>
        /// <returns></returns>
        public float GetVertical()
        {
            return normalizedStickVector.y;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            Assert.IsNotNull(raycastTarget);
            Assert.IsNotNull(stickImage);

            rectTransform = GetComponent<RectTransform>();
            raycastTargetRectTransform = raycastTarget.GetComponent<RectTransform>();
            stickImageRectTransform = stickImage.rectTransform;

            raycastTarget.OnBeginDragEvent.AddListener(eventData =>
            {
                defaultPos = raycastTargetRectTransform.localPosition;
            });

            raycastTarget.OnDragEvent.AddListener(eventData =>
            {
                Vector2 localPos;
                var result = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform,
                    eventData.position,
                    UiCamera,
                    out localPos);
                raycastTargetRectTransform.localPosition = localPos;

                var vec = localPos - (Vector2)defaultPos;
                var dir = vec.normalized;
                var mag = vec.magnitude;
                if (mag > stickRange)
                {
                    mag = stickRange;
                }
                stickVector = mag * dir;
                stickImageRectTransform.localPosition = defaultPos + (Vector3)stickVector;
                normalizedStickVector = stickVector.normalized;
            });

            raycastTarget.OnEndDragEvent.AddListener(eventData =>
            {
                raycastTargetRectTransform.localPosition = defaultPos;
            });
        }
    }
}