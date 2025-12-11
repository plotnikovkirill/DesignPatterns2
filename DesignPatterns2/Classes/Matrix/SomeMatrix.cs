using DesignPatterns2.Classes.VectorClasses;
using DesignPatterns2.Interfaces;

namespace DesignPatterns2.Classes.Matrix
{
    abstract class SomeMatrix: IMatrix
    {
        protected List<IVector> _components;

        public SomeMatrix()
        {
            _components = new List<IVector>();
        }

        public int RowNum => _components.Count;

        public int ColumnNum => _components[0] != null ? _components[0].VectorDimensionality : 0;
        

        public void SetElement(int indexX, int indexY, float newValue)
        {
            if (IndexInRange(indexX, indexY))
            {
                _components[indexX].SetComponent(indexY, newValue);
            }
        }

        public float GetElement(int indexX, int indexY)
        {
            if (IndexInRange(indexX, indexY))
            {
                return _components[indexX].GetComponent(indexY);
            }
            return 0;
        }

        private bool IndexInRange(int indexX, int indexY)
        {
            if (RowNum == 0) return false;
            return indexX >= 0 && indexX < RowNum && indexY >= 0 && indexY < ColumnNum;
        }

    }
}
