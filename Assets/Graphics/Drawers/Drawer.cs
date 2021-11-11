using Assets.Graphics.Shapes;
using Assets.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Assets.Positional;

namespace Assets.Graphics.Drawers
{
    class Drawer
    {
        protected Player aligner;
        public Drawer(Player aligner)
        {
            this.aligner = aligner;
        }

        public virtual void Draw(Shape s)
        {

        }
    }

    class AISDrawer : Drawer
    {
        // VS was complaining, so I added this, but it shouldn't be necessary...
        public AISDrawer(Player aligner) : base(aligner)
        {
            this.aligner = aligner;
        }

        public override void Draw(Shape s)
        {
            AISShape shape = (AISShape)s;

            foreach (Tuple<AISDTO, GameObject> obj in shape.objects.Values) 
            {
                if (obj.Item1.Target) InjectDTO(obj.Item1, obj.Item2);
            }
        }

        private void InjectDTO(AISDTO dto, GameObject obj)
        {
            string name = dto.Name.Length > 16 ? dto.Name.Substring(0, 16) : dto.Name;
            FillTextField("Name", name, obj);
            FillTextField("HDGValue", dto.Heading.ToString(), obj);
            FillTextField("COGValue", dto.COG.ToString(), obj);
            FillTextField("SOGValue", dto.SOG.ToString(), obj);
            FillTextField("CPAValue", "Test", obj);
        }

        private void FillTextField(string fname, string value, GameObject g)
        {
            GameObject obj = g.transform.Find($"StickAnchor/Stick/PinAnchor/AISPinTarget/Canvas/{fname}").gameObject;
            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = value;
        }
    }

}
