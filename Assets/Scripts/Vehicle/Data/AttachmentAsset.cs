using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRigs.Vehicle
{
    [CreateAssetMenu(menuName = "Vehicle/AttachmentAsset")]
    public class AttachmentAsset : ScriptableObject
    {
        public string id;
        public GameObject prefab;
        public int width;
        public int height;
    }
}