#pragma warning disable CS0649
/**************************************************
 *  DataTableBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public class DataTableBehaviour
        : UIHelperBehaviour
    {
        [SerializeField]
        private DataRowBehaviour dataRowPrefab;

        [SerializeField]
        private DataFieldBehaviour dataFieldPrefab;

        [SerializeField]
        private DataRowBehaviour headerRow;

        [SerializeField]
        private List<DataFieldDefinition> dataFieldDefinitions = new List<DataFieldDefinition>();

        private List<DataRowBehaviour> rows;

        private IEnumerable dataSource;

        public DataRowBehaviour DataRowPrefab
        {
            get
            {
                if (this.dataRowPrefab == null)
                {
                    throw new UIException($"The {nameof(this.dataRowPrefab)} property needs to be set in the Unity editor.");
                }

                return this.dataRowPrefab;
            }
        }

        public DataFieldBehaviour DataFieldPrefab
        {
            get
            {
                if (this.dataFieldPrefab == null)
                {
                    throw new UIException($"The {nameof(this.dataFieldPrefab)} property needs to be set in the Unity editor.");
                }

                return this.dataFieldPrefab;
            }
        }

        public List<DataRowBehaviour> Rows
        {
            get
            {
                if (this.rows == null)
                {
                    this.rows = new List<DataRowBehaviour>(new[] { this.headerRow });
                }

                return this.rows;
            }
        }

        public IDataBoundTable<TDataType> GetBindingInterface<TDataType>()
        {
            return this.GetComponent<IDataBoundTable<TDataType>>();
        }
    }
}
