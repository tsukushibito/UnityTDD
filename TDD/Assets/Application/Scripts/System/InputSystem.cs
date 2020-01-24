using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace App
{
    /// <summary>
    /// トリガータイプ
    /// </summary>
    public enum TriggerType
    {
        On,     /// 押した状態
        Down,   /// 押した瞬間
        Up,     /// 離した瞬間
    }

    /// <summary>
    /// 入力システム
    /// UnityのInputシステムのラッパー
    /// </summary>
    public class InputSystem : MonoBehaviour
    {
        /// <summary>
        /// アクションイベント
        /// </summary>
        public class ActionEvent : UnityEvent { }

        /// <summary>
        /// 軸イベント
        /// </summary>
        public class AxisEvent : UnityEvent<float> { }

        /// <summary>
        /// アクションイベントキー
        /// </summary>
        public struct ActionEventKey
        {
            public ActionCode code;
            public TriggerType trigger;

            public override int GetHashCode()
            {
                return code.GetHashCode() ^ trigger.GetHashCode();
            }
        }

        /// <summary>
        /// ログのタグ文字列
        /// </summary>
        private const string LogTag = "InputSystem";

        /// <summary>
        /// インスタンス
        /// </summary>
        /// <value></value>
        public static InputSystem Instance { get; private set; }

        [SerializeField]
        private ActionMapping initialActionMapping = null;

        [SerializeField]
        private AxisMapping initialAxisMapping = null;

        [SerializeField]
        private VirtualGamepad virtualGamepad = null;

        /// <summary>
        /// マッピングデータ
        /// </summary>
        private Stack<ActionMapping> actionMappingStack = new Stack<ActionMapping>();

        private AxisMapping axisMapping = null;

        private Dictionary<TriggerType, Func<KeyCode, bool>> getKeyDelegateTable = null;

        private Dictionary<TriggerType, Func<VirtualKeyCode, bool>> getVirtualKeyDelegateTable = null;

        /// <summary>
        /// イベントテーブル
        /// </summary>
        private Dictionary<ActionEventKey, ActionEvent> eventTable = new Dictionary<ActionEventKey, ActionEvent>();

        /// <summary>
        /// コンフィグプロパティ
        /// </summary>
        /// <value></value>
        public Stack<ActionMapping> MappingDataStack { get { return actionMappingStack; } }

        /// <summary>
        /// イベントリスナー追加
        /// </summary>
        /// <param name="code"></param>
        /// <param name="trigger"></param>
        /// <param name="action"></param>
        public void AddActionEventListener(ActionCode code, TriggerType trigger, UnityAction action)
        {
            var key = new ActionEventKey { code = code, trigger = trigger };
            ActionEvent inputEvent;
            if (!eventTable.TryGetValue(key, out inputEvent))
            {
                inputEvent = new ActionEvent();
                eventTable.Add(key, inputEvent);
            }

            Assert.IsNotNull(inputEvent, "inputEvent must be not null!");

            inputEvent.AddListener(action);
        }

        /// <summary>
        /// イベントリスナー削除
        /// </summary>
        /// <param name="code"></param>
        /// <param name="trigger"></param>
        /// <param name="action"></param>
        public void RemoveActionEventListener(ActionCode code, TriggerType trigger, UnityAction action)
        {
            var key = new ActionEventKey { code = code, trigger = trigger };
            ActionEvent inputEvent;
            if (!eventTable.TryGetValue(key, out inputEvent))
            {
                Debug.unityLogger.LogWarning(LogTag, "Trying to remove listener at empty action.");
                return;
            }

            Assert.IsNotNull(inputEvent, "inputEvent must be not null!");

            inputEvent.RemoveListener(action);

            // リスナーが空なら、テーブルから取り除く
            if (inputEvent.GetPersistentEventCount() == 0)
            {
                eventTable.Remove(key);
            }
        }

        /// <summary>
        /// 入力に応じたイベント実行
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inputEvent"></param>
        private void HandleInput(ActionEventKey key, ActionEvent inputEvent)
        {
            ActionMapping.MappingData actionMappingData;

            bool hasHandledActionInput = false;
            foreach (var mappingData in actionMappingStack)
            {
                if (mappingData.MappingDataTable.TryGetValue(key.code, out actionMappingData))
                {
                    HandleActionInput(key, actionMappingData, inputEvent);
                    hasHandledActionInput = true;
                    break;
                }
                else
                {
                    if (mappingData.CanBlockInput) break;
                }
            }

            if (hasHandledActionInput == false)
            {
                // Debug.unityLogger.LogWarning(LogTag, string.Format("There is no mapping data. ({0})", key.code));
            }
        }

        /// <summary>
        /// Keyタイプの入力処理
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mappingData"></param>
        /// <param name="inputEvent"></param>
        private void HandleActionInput(ActionEventKey key, ActionMapping.MappingData mappingData, ActionEvent inputEvent)
        {
            var keyCodeList = mappingData.keyCodeList;
            var virtualKeyCodeList = mappingData.virtualKeyCodeList;

            foreach (var code in keyCodeList)
            {
                if (getKeyDelegateTable[key.trigger](code))
                {
                    inputEvent.Invoke();
                    return;
                }
            }

            foreach (var code in virtualKeyCodeList)
            {
                if (getVirtualKeyDelegateTable[key.trigger](code))
                {
                    inputEvent.Invoke();
                    return;
                }
            }
        }

        /// <summary>
        /// Axisタイプの入力処理
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mappingData"></param>
        /// <param name="inputEvent"></param>
        private void HandleAxisInput(ActionEventKey key, AxisMapping.MappingData mappingData, AxisEvent inputEvent)
        {
            if (!string.IsNullOrEmpty(mappingData.axisName))
            {
                var axis = Input.GetAxis(mappingData.axisName);
                inputEvent.Invoke(axis);
            }
            else if (mappingData.virtualAxis != VirtualAxisCode.None)
            {
                var axis = virtualGamepad.GetAxis(mappingData.virtualAxis);
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            Assert.IsNull(Instance, "InputSystem instance already exists!");
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;

            if (initialActionMapping == null)
            {
                initialActionMapping = ScriptableObject.CreateInstance<ActionMapping>();
            }
            initialActionMapping.SetupMappingTable();
            actionMappingStack.Push(initialActionMapping);

            if (initialAxisMapping == null)
            {
                initialAxisMapping = ScriptableObject.CreateInstance<AxisMapping>();
            }
            initialAxisMapping.SetupMappingTable();
            axisMapping = initialAxisMapping;

            getKeyDelegateTable = new Dictionary<TriggerType, Func<KeyCode, bool>>();
            getKeyDelegateTable.Add(TriggerType.On, Input.GetKey);
            getKeyDelegateTable.Add(TriggerType.Down, Input.GetKeyDown);
            getKeyDelegateTable.Add(TriggerType.Up, Input.GetKeyUp);

            getVirtualKeyDelegateTable = new Dictionary<TriggerType, Func<VirtualKeyCode, bool>>();
            getVirtualKeyDelegateTable.Add(TriggerType.On, virtualGamepad.GetKey);
            getVirtualKeyDelegateTable.Add(TriggerType.Down, virtualGamepad.GetKeyDown);
            getVirtualKeyDelegateTable.Add(TriggerType.Up, virtualGamepad.GetKeyUp);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void Update()
        {
            foreach (var keyValue in eventTable)
            {
                Assert.IsNotNull(keyValue.Value);
                HandleInput(keyValue.Key, keyValue.Value);
            }
        }
    }

}