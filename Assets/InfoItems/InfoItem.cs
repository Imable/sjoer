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

        private TargettableInfoItem targetHandler;

        public InfoItem(DTO dto, DataType dataType, DisplayArea displayArea)
        {
            this.meta = new Meta(dto.Target, 0, dataType, displayArea);
            this.dto  = dto;
            //Update();
        }

        public override bool Equals(System.Object i)
        {
            var other = i as InfoItem;
            return this.Key == other.Key;
        }

        public bool IsTarget
        {
            get { return this.meta.Target; }
            set { this.meta.Target = value; }
        }

        public DataType DataType
        {
            get { return this.meta.DataType; }
        }

        public DisplayArea DisplayArea
        {
            get { return this.meta.DisplayArea; }
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
            if (GetTargetHandler(true))
            {
                if (GetTargetHandler().IsTarget)
                {
                    Debug.Log("Target on " + Key);

                }
            }
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
            this.meta.Target = this.dto.Target || (GetTargetHandler() && GetTargetHandler().IsTarget);
        }

        public TargettableInfoItem GetTargetHandler(bool forceUpdate = false)
        {
            // If the target changes, a new GameObject is created in ShapeProvider, which forces and update of `targetHandler`
            if (!targetHandler || forceUpdate)
            {
                targetHandler = this.gameObject ? this.gameObject.GetComponent<TargettableInfoItem>() : null;
            }
            return targetHandler;
        }

        public bool TargetHasChanged()
        {
            return this.meta.Target != this.meta.PreviousTarget;
        }

        protected virtual void Reshape()
        {
            GraphicFactory.Instance.getShapeProvider(meta.DataType, meta.DisplayArea).Get(this);
        }

        protected virtual void Refill()
        {
            GraphicFactory.Instance.GetFiller(meta.DataType, meta.DisplayArea, this.IsTarget).Fill(this);
        }

        protected virtual void Reposition()
        {
            GraphicFactory.Instance.getPositioner(meta.DataType, meta.DisplayArea).Position(this);
        }

        // Called on the old InfoItem
        public void InjectNewDTO(DTO dto)
        {
            this.meta.PreviousTarget = this.meta.Target;
            this.dto = dto;
        }

        // Called on the InfoItem that contains the link
        public void LinkTargetHandler(InfoItem infoItem)
        {
            TargettableInfoItem handler = GetTargetHandler();
            TargettableInfoItem linkedHandler = infoItem.GetTargetHandler();
            handler.SetLink(linkedHandler);
            // Equalize this and linked target status
            handler.IsTarget = linkedHandler.IsTarget;
        }

        public void DestroyMesh()
        {
            UnityEngine.Object.Destroy(this.gameObject);
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
                AISDTO aisDTO = aisDTOs.vessels[i];
                if (aisDTO.Valid)
                {
                    yield return new AISInfoItem(aisDTOs.vessels[i], dataType, displayArea);
                }
            }
        }
    }
}

