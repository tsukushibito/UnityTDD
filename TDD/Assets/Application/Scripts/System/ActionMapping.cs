using System.Collections.Generic;
using UnityEngine;

namespace App
{
    /// <summary>
    /// 入力コンフィグ
    /// </summary>
    [CreateAssetMenu(fileName = "ActionMapping", menuName = "App/ActionMapping")]
    public class ActionMapping : ScriptableObject
    {
        /// <summary>
        /// アクションマッピングデータ
        /// </summary>
        [System.Serializable]
        public struct MappingData
        {
            public string name;
            public ActionCode action;
            public List<KeyCode> keyCodeList;
            public List<VirtualKeyCode> virtualKeyCodeList;
        }

        /// <summary>
        /// マッピングデータリスト
        /// </summary>
        /// <typeparam name="MappingData"></typeparam>
        /// <returns></returns>
        [SerializeField]
        private List<MappingData> mappingDataList = new List<MappingData>();

        /// <summary>
        /// マッピングテーブル
        /// </summary>
        /// <typeparam name="ActionCode"></typeparam>
        /// <typeparam name="MappingData"></typeparam>
        /// <returns></returns>
        private Dictionary<ActionCode, MappingData> mappingDataTable = new Dictionary<ActionCode, MappingData>();

        /// <summary>
        /// スタック下層マッピングデータへのイベントを遮断するか
        /// </summary>
        [SerializeField]
        private bool canBlockInput = false;

        /// <summary>
        /// マッピングテーブルプロパティ
        /// </summary>
        /// <value></value>
        public Dictionary<ActionCode, MappingData> MappingDataTable { get { return mappingDataTable; } }

        public bool CanBlockInput { get { return canBlockInput; } }

        /// <summary>
        /// マッピングテーブルのセットアップ
        /// InputSystemではMappingTableを参照するので、このメソッドを呼ばないと設定が反映されません
        /// </summary>
        public void SetupMappingTable()
        {
            mappingDataTable.Clear();
            foreach (var data in mappingDataList)
            {
                mappingDataTable.Add(data.action, data);
            }
        }

        /// <summary>
        /// 有効化処理
        /// </summary>
        private void OnEnabled()
        {
            SetupMappingTable();
        }
    }

}