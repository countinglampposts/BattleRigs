using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRigs.Vehicle
{
    public class HardpointGrid : MonoBehaviour
    {
        public string id;

        public int widthCount;
        public int heightCount;

        public Vector3 GetHardpointPosition(int x, int y)
        {
            float width = widthCount - 1;
            float height = heightCount - 1;

            var p1 = new Vector3(width / 2, height / 2);
            var p2 = new Vector3(-width / 2, height / 2);
            var p3 = new Vector3(-width / 2, -height / 2);

            var xLerp = (float)x / (widthCount - 1);
            var yLerp = (float)y / (heightCount - 1);
            var localPosition = new Vector3(Mathf.Lerp(p1.x, p2.x, xLerp), Mathf.Lerp(p1.y, p3.y, yLerp));
            return transform.TransformPoint(localPosition);
        }

        private void OnDrawGizmosSelected()
        {
            float width = widthCount - 1;
            float height = heightCount - 1;

            var p1 = new Vector3(width / 2, height / 2);
            var gp1 = transform.TransformPoint(p1);
            var p2 = new Vector3(-width / 2, height / 2);
            var gp2 = transform.TransformPoint(p2);
            var p3 = new Vector3(-width / 2, -height / 2);
            var gp3 = transform.TransformPoint(p3);
            var p4 = new Vector3(width / 2, -height / 2);
            var gp4 = transform.TransformPoint(p4);

            Gizmos.DrawLine(gp1, gp2);
            Gizmos.DrawLine(gp2, gp3);
            Gizmos.DrawLine(gp3, gp4);
            Gizmos.DrawLine(gp4, gp1);

            // p1----p2
            // |     |
            // |     |
            // p3----p4

            for (var x = 0; x < widthCount; x++)
            {
                for (var y = 0; y < heightCount; y++)
                {
                    var position = GetHardpointPosition(x, y);
                    Gizmos.DrawWireSphere(position, .5f);
                }
            }
        }
    }
}