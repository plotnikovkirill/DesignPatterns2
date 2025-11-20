using DesignPatterns2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Classes.VectorClasses
{
    internal class RegularVector: IVector
    {
        List<float> components = new List<float>();
        public int VectorDimensionality
        {
            get
            {
                return components.Count;
            }
        }

        public RegularVector (params float[] newComponents)
        {
            components.AddRange(newComponents);
        }
        public float GetComponent(int index)
        {
            if (!IndexInRange(index))
                return 0;
            return components[index];
        }

        public void SetComponent(int index, float value)
        {
            if (!IndexInRange(index))
                return;
            components[index] = value;
        }

        private bool IndexInRange(int index)
        {
            return index < VectorDimensionality && index >= 0;
        }
    }
}
