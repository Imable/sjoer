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
        protected WorldAligner aligner;
        public Drawer(WorldAligner aligner)
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
        public AISDrawer(WorldAligner aligner) : base(aligner)
        {
            this.aligner = aligner;
        }

        public override void Draw(Shape s)
        {
            AISShape shape = (AISShape)s;

            foreach (Tuple<AISDTO, GameObject> obj in shape.objects.Values) 
            {
                InjectDTO(obj.Item1, obj.Item2);
            }
        }

        private void InjectDTO(AISDTO dto, GameObject obj)
        {
            FillTextField("Name", dto.Name, obj);
        }

        private void FillTextField(string fname, string value, GameObject g)
        {
            GameObject obj = g.transform.Find($"TextField/{fname}").gameObject;
            TextMeshPro tmp = obj.GetComponent<TextMeshPro>();
            tmp.text = value;
        }
    }

}
