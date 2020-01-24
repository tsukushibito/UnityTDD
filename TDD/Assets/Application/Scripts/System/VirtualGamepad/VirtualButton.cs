using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App
{
    /// <summary>
    /// 仮想ボタン
    /// </summary>
    public class VirtualButton : Selectable, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// 前フレームに押下されているか？
        /// </summary>
        private bool isPressedPrevFrame = false;

        private UnityEvent onClickEvent = new UnityEvent();

        /// <summary>
        /// ボタン画像
        /// </summary>
        private Image buttonImage = null;

        /// <summary>
        /// ボタンが押されたときのイベント
        /// </summary>
        /// <value></value>
        public UnityEvent OnClickEvent { get { return onClickEvent; } }

        /// <summary>
        /// ボタンが押されているか？
        /// </summary>
        /// <returns></returns>
        public bool GetButton()
        {
            return IsPressed();
        }

        /// <summary>
        /// ボタンが押された瞬間か
        /// </summary>
        /// <returns></returns>
        public bool GetButtonDown()
        {
            return IsPressed() && !isPressedPrevFrame;
        }

        /// <summary>
        /// ボタンが離された瞬間か
        /// </summary>
        /// <returns></returns>
        public bool GetButtonUp()
        {
            return !IsPressed() && isPressedPrevFrame;
        }

        /// <summary>
        /// 指を離したときの処理
        /// IPointerUpHandlerの実装
        /// </summary>
        /// <param name="data"></param>
        public override void OnPointerUp(PointerEventData data)
        {
            base.OnPointerUp(data);
        }

        /// <summary>
        /// タップした瞬間の処理
        /// IPointerDownHandlerの実装
        /// </summary>
        /// <param name="data"></param>
        public override void OnPointerDown(PointerEventData data)
        {
            base.OnPointerDown(data);
            onClickEvent.Invoke();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private new void Awake()
        {
            base.Awake();

            buttonImage = GetComponent<Image>();
        }

        /// <summary>
        /// 後更新処理
        /// </summary>
        private void LateUpdate()
        {
            isPressedPrevFrame = IsPressed();
        }
    }
}