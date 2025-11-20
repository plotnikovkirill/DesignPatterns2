using DesignPatterns2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns2.Classes.VectorClasses
{
    internal class RAZVector: IVector
    {
        private Dictionary<int, float> notZeroComponents = new Dictionary<int, float>();
        private int dimensionality;
        public int VectorDimensionality => dimensionality;

        public RAZVector(params float[] components)
        {
            dimensionality = components.Length;
            for (int i = 0; i < components.Length; ++i)
            {
                if (components[i] != 0)
                {
                    notZeroComponents.Add(i, components[i]);
                }
            }
        }

        public float GetComponent (int index)
        {
            if (!IndexInRange(index))
                return 0;
            return notZeroComponents.ContainsKey(index) ? notZeroComponents[index] : 0;
        }

        public void SetComponent(int index, float newValue)
        {
            if (!IndexInRange(index))
                return;

            if (newValue == 0)
            {
                notZeroComponents.Remove(index);
            }
            else
            {
                notZeroComponents[index] = newValue;
            }
        }

        private bool IndexInRange(int index)
        {
            return index >= 0 && index < VectorDimensionality;
        }

    }
}
