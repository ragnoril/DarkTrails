using UnityEngine;
using System.Collections;

namespace DarkTrails.Tools
{

    public class PointsBaseWindow : MonoBehaviour
    {
        public int PointsHave;
        public int PointsSpent;
        
        public virtual void UpdateStats() { }
        
    }

}
