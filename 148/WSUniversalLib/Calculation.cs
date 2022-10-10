using System;

namespace WSUniversalLib
{
    public class Calculation
    {
        int GetQuantityForProduct(int productType, int materialType, int count, float width, float length)
        {
            float materialCoeff = 1;
            float productTypeCoeff = 1;

            switch (materialType)
            {
                case 1: { materialCoeff = 1.03f; break; }
                case 2: { materialCoeff = 1.12f; break; }
            }

            switch (productType)
            {
                case 1: { productTypeCoeff = 1.1f; break; }
                case 2: { productTypeCoeff = 2.5f; break; }
                case 3: { productTypeCoeff = 8.43f; break; }
            }

            double amount = width * length * materialCoeff * productTypeCoeff * count;

            return (int)Math.Ceiling(amount);
        }
    }
}
