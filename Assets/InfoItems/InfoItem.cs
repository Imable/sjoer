using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.DataManagement;
using UnityEngine;
using Unity;
using Assets.Positional;
using Assets.Graphics;

namespace Assets.InfoItems
{
    class InfoItem
    {
        protected DTO dto;
        protected GameObject gameObject;
        protected Meta meta;


        public InfoItem(DTO dto, DataType dataType, DisplayArea displayArea)
        {
            this.meta = new Meta(dto.Target, 0, dataType, displayArea);
            this.dto  = dto;
        }

        public bool IsTarget
        {
            get { return this.meta.Target; }
        }

        public virtual string Key
        {
            get { return dto.Key; }
        }

        public GameObject Shape
        {
            get { return gameObject; }
            set { gameObject = value; }
        }

        public DTO GetDTO
        {
            get { return dto; }
        }

        public static IEnumerable<InfoItem> Generate(DTO dto, DataType dataType, DisplayArea displayArea)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            // First update the target from interactions
            Retarget();
            // Get new shape
            Reshape();
            // Fill new shape if necessary
            Refill();
            // Positional shape
            Reposition();
        }

        // Update target in meta according to selected in scene or selected previously
        protected void Retarget ()
        {
            this.meta.Target = this.meta.Target || (GetTargetHandler() && GetTargetHandler().IsTarget);
        }

        public TargettableInfoItem GetTargetHandler()
        {
            return this.gameObject ? this.gameObject.GetComponent<TargettableInfoItem>() : null;
        }

        public bool TargetHasChanged()
        {
            return this.meta.Target != this.meta.PreviousTarget;
        }

        protected virtual void Reshape()
        {
            GraphicFactory.Instance.getShapeProvider(meta.DataType).Get(this);
        }

        protected virtual void Refill()
        {
            if (IsTarget) GraphicFactory.Instance.GetFiller(meta.DataType).Fill(this);
        }

        protected virtual void Reposition()
        {
            GraphicFactory.Instance.getPositioner(meta.DataType).Position(this, meta.DisplayArea);
        }

        // Called on the new InfoItem
        public void Merge(InfoItem oldInfoItem)
        {
            this.meta.PreviousTarget = oldInfoItem.IsTarget;
            this.gameObject = oldInfoItem.gameObject;
        }
    }

    class AISInfoItem : InfoItem
    {
        public AISInfoItem(DTO dto, DataType dataType, DisplayArea displayArea) : base(dto, dataType, displayArea)
        {
        }

        public static new IEnumerable<InfoItem> Generate(DTO dto, DataType dataType, DisplayArea displayArea)
        {
            AISDTOs aisDTOs = (AISDTOs)dto;

            for (int i = 0; i < aisDTOs.vessels.Length; i++)
            {
                yield return new AISInfoItem(aisDTOs.vessels[i], dataType, displayArea);
            }
        }
    }
}

