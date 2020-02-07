using System.Collections.Generic;
using UnityEngine;

namespace App
{
    /// <summary>
    /// 入力コンフィグ
    /// </summary>
    [CreateAssetMenu(fileName = "AxisMapping", menuName = "Mag/AxisMapping")]
    public class AxisMapping : ScriptableObject
    {
        /// <summary>
        /// マッピングデータ
        /// </summary>
        [System.Serializable]
        public struct MappingData
        {
            public string name;
            public AxisCode axis;
            public string axisName;
            public VirtualAxisCode virtualAxis;
        }

        [SerializeField]
        private List<MappingData> mappingDataList = new List<MappingData>();

        private Dictionary<AxisCode, MappingData> mappingDataTable = new Dictionary<AxisCode, MappingData>();

        /// <summary>
        /// マッピングテーブルプロパティ
        /// </summary>
        /// <value></value>
        public Dictionary<AxisCode, MappingData> MappingDataTable { get { return mappingDataTable; } }

        /// <summary>
        /// マッピングテーブルのセットアップ
        /// InputSystemではMappingTableを参照するので、このメソッドを呼ばないと設定が反映されません
        /// </summary>
        public void SetupMappingTable()
        {
            mappingDataTable.Clear();
            foreach (var data in mappingDataList)
            {
                mappingDataTable.Add(data.axis, data);
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