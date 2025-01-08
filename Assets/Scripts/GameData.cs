using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [System.Serializable]
    public class GameData
    {
        public double CurrentConeCount;
        public double CurrentFragmentCount;
        public double CurrentConePerSecond;
        public double ConesPerClickUpgrade;
        public List<double> UpgradeCosts; // Store upgrade costs
    }
}



