using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PathClass
{
    [Serializable]

    public class pathSegment
    {
        public bool moving;
        public Vector3 point_A, point_B;
        public bool a_reached, b_reached;
        public turnArounds[] turns;
        public bool allTurnsComplete;
    }

    [Serializable]
    public class turnArounds
    {
        //public Quaternion rotateTo;
        public Vector3 rotateTo;
        public float rotateSpeed;
        //public float rotationTotalTime;
        public enum axis { xTurn, yTurn, zTurn};
        public axis turnAxis = axis.xTurn;
        public bool clockwise;
        public bool turning;
        public bool turnComplete;
    }
}
