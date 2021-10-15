using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;
using TMPro;
using System.IO;
using Assets.DataManagement;

namespace Assets.Graphics.Shapes
{
    
    class Shape
    {
        protected GameObject shape;

        public virtual void Get(DTO dto)
        {

        }

        protected void GetShape(string fname)
        {
            UnityEngine.Object shape = LoadPrefabFromFile(fname);
            this.shape = (GameObject) GameObject.Instantiate(shape, Vector3.zero, Quaternion.identity);
        }

        // Source: https://answers.unity.com/questions/313398/is-it-possible-to-get-a-prefab-object-from-its-ass.html
        private UnityEngine.Object LoadPrefabFromFile (string fname)
        {
            UnityEngine.Object obj = Resources.Load(fname);
            if (obj == null)
            {
                throw new FileNotFoundException($"Cannot load prefab {fname}");
            }
            return obj;
        }
    }

    class AISShape : Shape
    {
        private List<GameObject> gameObjects = new List<GameObject>();

        public override void Get(DTO dto)
        {
            AISDTOs aisDTOs = (AISDTOs) dto;

            foreach (AISDTO aisDTO in aisDTOs.vessels)
            {
                
            }
        }
    }
}
