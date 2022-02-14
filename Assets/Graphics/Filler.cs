using Assets.DataManagement;
using Assets.InfoItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Graphics
{
    class Filler
    {
        public bool Target { get; set; }

        public virtual void Fill(InfoItem infoItem)
        {
            string name = "";
            if (infoItem.IsTarget) name += "[T] ";
            name += $"{infoItem.Key} ({infoItem.DisplayArea})";

            infoItem.Shape.name = name;
        }

        protected virtual void TargetFiller(InfoItem infoItem) { }
        protected virtual void NonTargetFiller(InfoItem infoItem) { }


        protected void FillTextField(string fname, string value, GameObject g)
        {
            GameObject obj = g.transform.Find($"StickAnchor/Stick/PinAnchor/AISPinTarget/ShipIconAnchor/Canvas/{fname}").gameObject;
            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = value;
        }
    }

    class AISHorizonFiller : Filler
    {
        public override void Fill(InfoItem infoItem)
        {
            base.Fill(infoItem);
            if (Target) TargetFiller(infoItem);
            else NonTargetFiller(infoItem);

        }

        protected override void NonTargetFiller(InfoItem infoItem)
        {

        }

        protected override void TargetFiller(InfoItem infoItem)
        {
            AISDTO dto = (AISDTO)infoItem.GetDTO;
            FillTextField("1Label", "SOG", infoItem.Shape);
            FillTextField("1Value", dto.SOG.ToString(), infoItem.Shape);
            FillTextField("2Label", "HDG", infoItem.Shape);
            FillTextField("2Value", dto.Heading.ToString() + "°", infoItem.Shape);
            FillTextField("TargetNum", infoItem.TargetNum.ToString(), infoItem.Shape);
        }
    }

    class AISSkyFiller : Filler
    {
        public override void Fill(InfoItem infoItem)
        {
            base.Fill(infoItem);
            if (Target) TargetFiller(infoItem);
            else NonTargetFiller(infoItem);
        }

        protected override void TargetFiller(InfoItem infoItem)
        {
            AISDTO dto = (AISDTO)infoItem.GetDTO;

            string name = dto.Name.Length > 16 ? dto.Name.Substring(0, 16) : dto.Name;
            FillTextField("Name", name, infoItem.Shape);
            FillTextField("HDGValue", dto.Heading.ToString(), infoItem.Shape);
            FillTextField("COGValue", dto.COG.ToString(), infoItem.Shape);
            FillTextField("SOGValue", dto.SOG.ToString(), infoItem.Shape);
            FillTextField("DRGValue", dto.Draught.ToString(), infoItem.Shape);
        }
    }
}
