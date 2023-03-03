using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev.Core.AI
{
    public class Indicator : MonoBehaviour
    {
        [SerializeField] MeshFilter _MeshFilter;

        public MeshFilter GetMeshFilter() => _MeshFilter;
    }
}
