/**************************************************
 *  DataRowBehaviour.cs
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

    public class DataRowBehaviour
        : UIHelperBehaviour
    {
        private Selectable selectable;

        public DataTableBehaviour DataTable { get; set; }

        public List<DataFieldBehaviour> Fields { get; } = new List<DataFieldBehaviour>();

        public float Height { get => this.GetComponent<RectTransform>().rect.height; }

        public float XPosition { get => this.transform.localPosition.x; }

        public float YPosition { get => this.transform.localPosition.y; }

        public Selectable Selectable
        {
            get
            {
                if (this.selectable == null)
                {
                    this.selectable = this.GetComponent<Selectable>();
                }

                return this.selectable;
            }
        }

        public IDataBoundRow<TDataType> GetBindingInterface<TDataType>()
        {
            return this.GetComponent<IDataBoundRow<TDataType>>();
        }

        public void Select()
        {
            var selectable = this.GetComponent<Selectable>();

            if (selectable != null)
            {
                selectable.Select();
            }
        }
    }
}
