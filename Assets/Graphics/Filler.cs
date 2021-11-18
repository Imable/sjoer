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
        public virtual void Fill(InfoItem infoItem)
        {
            throw new NotImplementedException();
        }
    }

    class AISFiller : Filler
    {
        public override void Fill(InfoItem infoItem)
        {
            TargetFiller(infoItem);
        }

        private void TargetFiller(InfoItem infoItem)
        {
            AISDTO dto = (AISDTO)infoItem.GetDTO;

            string name = dto.Name.Length > 16 ? dto.Name.Substring(0, 16) : dto.Name;
            FillTextField("Name", name, infoItem.Shape);
            FillTextField("HDGValue", dto.Heading.ToString(), infoItem.Shape);
            FillTextField("COGValue", dto.COG.ToString(), infoItem.Shape);
            FillTextField("SOGValue", dto.SOG.ToString(), infoItem.Shape);
            FillTextField("DRGValue", dto.Draught.ToString(), infoItem.Shape);
        }

        private void FillTextField(string fname, string value, GameObject g)
        {
            GameObject obj = g.transform.Find($"StickAnchor/Stick/PinAnchor/AISPinTarget/Canvas/{fname}").gameObject;
            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = value;
        }
    }
}
