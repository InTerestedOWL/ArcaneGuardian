using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Control
{
    public class POIBuildingState : AiState
    {
        public AiStateId GetId()
        {
            return AiStateId.POIBuilding;
        }

        public void Enter(StateMachineController controller)
        {
            //TODO: Remove POI from building action abr
            //TODO: Play Placing animation
            Debug.Log("Building state");
            //TODO: Place POI on grid
            GameObject poi_building = GameObject.FindWithTag("POI_Building");
            Vector3 tar_pos = poi_building.transform.position;
            controller.movement.DoMovement(tar_pos);
            
            Debug.Log("I have arrived in yo mama");
            //IDEE: POI Grid erzeugen lassen, damit nur beim POI gebaut werden kann?
            //Beim aufheben kann dann einfach das grid mit allen gebauten objekten disabled werden
            GameObject Grid = GameObject.Find("Grid");
            BuildingSystem bs = Grid.GetComponent<BuildingSystem>();
            
        }

        public void Update(StateMachineController controller)
        {
            if(Vector3.Distance(controller.transform.position,controller.movement.navMeshAgent.destination) < 0.1){
                Debug.Log("I have arrived in yo mama");
            }
        }

        public void Exit(StateMachineController controller)
        {
        }
    }
}
