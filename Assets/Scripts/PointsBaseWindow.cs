using UnityEngine;
using System.Collections;

namespace DarkTrails.UI
{

    public class PointsBaseWindow : MonoBehaviour
    {
        public int PointsHave;
        public int PointsSpent;
        
        public virtual void UpdateStats() { }
        
    }

}
