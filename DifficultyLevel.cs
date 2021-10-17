using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DifficultyLevel
{
    [Serializable]
    public class dLev
    {
        public string name;
        public int level;
        public float movementSpeed;
        public int notesToNextStage;
        [Range (0,10)]
        public int holdChance;
        [Range(0,10)]
        public int tapChance;
        public float refreshRate;

    }
}

