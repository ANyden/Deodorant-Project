using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PathClass
{
    [Serializable]

    public class pathSegment
    {
        public Vector3 point_A, point_B;
        public bool a_reached, b_reached;
        public turnArounds[] turns;
    }

    [Serializable]
    public class turnArounds
    {
        public Quaternion rotateTo;
        public float rotateSpeed;
        public float rotateTime;
    }
}
