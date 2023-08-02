// This readme is written by Malhar Choure

// this file contains important information regarding how the AI works specifically the pathfinding and movement of mobs such as enemies, bosses and boss spawns.

///<see cref="AstarPath">
///The script pathfinder on the aStar object in hierarchy is necessary to generate the graph upon which the enemies tread. This is required as this forms the basis of pathfinding for the A star algorithm
///We have used Layered Grid Graph of the size of the entire arena including procedurally generated chunks. This only works on static objects so we have scanned the map only after it has been fully formed.
// The necessary scripts that are required to move an enemy with basic path finding

//The few necessary usually used components are Capsule collider, mesh renderer, mesh filter, transform, character controller.
//****** important note: Make sure that the Step Offset of the character controller is less than or equal to its radius + (height*2). If this is not the case the enemy will not move.

///<see cref="Pathfinding/Seeker">
///The above script is necessary to have to enemy tract the player and seek him out. You need not change any values on this script.

///<see cref="Funnel">
///The above script is an optimization on pathfinding and you simple need to have unwrap ticked and Traversable graphs to everything.

///<see cref="AIpath(2D,3D)">
///Define radius and height according to your object and keep can move ticked. set acceleration to custom and high alongside high rotation speed. This should help in stopping NPC's from falling down the edges. Do not change anything else

///<see cref="AI Destination Setter">
///This sets the destination of the AI.The endpoint and needs the actual player transform. Do not pass player root.

///<see cref="TrainingTarget">
///This script is basically to store health and shield values and kill the player if the health falls to 0.

