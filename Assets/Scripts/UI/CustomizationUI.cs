using System.Collections;
using System.Collections.Generic;
using BattleRigs.Vehicle;
using UnityEngine;
using Zenject;

namespace BattleRigs.UI
{
    public class CustomizationUI : MonoBehaviour
    {
        [Inject] HardpointGrid[] hardpointGrids;
        [Inject] Camera camera;
    }
}