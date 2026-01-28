using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Alex.Scripts
{
    public class CrowdControllerManager : MonoBehaviour
    {
        private const float CELL_SIZE = 1.5f;
        private static List<CrowdController> s_crowdControllers;
        private static SpatialHashGrid s_grid;

        [SerializeField]
        private bool debugDraw;

        public static void RegisterController(CrowdController crowdController)
        {
            s_grid ??= new SpatialHashGrid(CELL_SIZE);
            s_crowdControllers ??= new List<CrowdController>();
            s_crowdControllers.Add(crowdController);
        }

        private void Update()
        {
            foreach (var crowdController in s_crowdControllers)
            {
                crowdController.CustomUpdate();
            }
        }

        private void LateUpdate()
        {
            s_grid.Clear();

            foreach (var crowdController in s_crowdControllers)
            {
                foreach (var agent in crowdController.agents)
                {
                    s_grid.AddAgent(agent);
                }
            }
        }
        
        public static List<CrowdAgent> GetNeighbors(Vector3 position)
        {
            return s_grid.GetNeighbors(position);
        }

        public static int GetDensityAtPosition(Vector3 position) => s_grid.GetDensityAtPosition(position);

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!debugDraw)
                return;
            
            s_grid?.GizmosDrawGrid();
        }
#endif
    }
}