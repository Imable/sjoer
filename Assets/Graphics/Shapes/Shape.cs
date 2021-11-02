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
using Assets.Resources;

namespace Assets.Graphics.Shapes
{
    
    class Shape
    {
        public virtual void Get(DTO dto)
        {
            throw new NotImplementedException();
        }

        protected GameObject GetShape(string fname)
        {
            return (GameObject) GameObject.Instantiate(
                    LoadPrefabFromFile(fname), 
                    Vector3.zero, 
                    Quaternion.identity
                );
        }

        // Source: https://answers.unity.com/questions/313398/is-it-possible-to-get-a-prefab-object-from-its-ass.html
        protected UnityEngine.Object LoadPrefabFromFile (string fname)
        {
            UnityEngine.Object obj = AssetManager.Instance.objects["AISPin"];
            if (obj == null)
            {
                throw new FileNotFoundException($"Cannot load prefab {fname}");
            }
            Debug.Log($"Got obj {obj.name}");
            return obj;
        }
    }

    class AISShape : Shape
    {
        public Dictionary<string, Tuple<AISDTO, GameObject>> objects = new Dictionary<string, Tuple<AISDTO, GameObject>>();

        public override void Get(DTO dto)
        {
            AISDTOs aisDTOs = (AISDTOs) dto;

            HashSet<string> newKeys = new HashSet<string>();
            foreach (AISDTO aisDTO in aisDTOs.vessels)
            {
                newKeys.Add(aisDTO.Name);
                UpdateObjects(aisDTO);
            }

            RemoveObjects(newKeys);
            
        }

        private void RemoveObjects(HashSet<string> newKeys)
        {
            List<string> keys = objects.Keys.ToList();
            foreach (string key in keys)
            {
                // If the newly added key in the AIS data does not contain the old key from the program
                // Then we have to remove it from the program
                if (!newKeys.Contains(key))
                {
                    objects.Remove(key);
                }
            }
        }

        private void UpdateObjects(AISDTO dto)
        {
            if (objects.ContainsKey(dto.Name))
            {
                InjectNewDTO(dto);
            } else
            {
                AddNewDTO(dto);
            }
        }

        // Add the DTO and create a new GameObject
        private void AddNewDTO(AISDTO dto)
        {
            objects[dto.Name] = new Tuple<AISDTO, GameObject>(dto, GetShape("AISPin/AISPinPrefab"));
        }

        // Update the DTO while leaving GameObject intact
        private void InjectNewDTO(AISDTO dto)
        {
            objects[dto.Name] = new Tuple<AISDTO, GameObject>(dto, objects[dto.Name].Item2);
        }
    }
}
