using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/** Movement modes for Characters. */
//UENUM(BlueprintType)
public enum EMovementMode
{
    /** None (movement is disabled). */
    MOVE_None,   //    UMETA(DisplayName = "None"),

    /** Walking on a surface. */
    MOVE_Walking,  //  UMETA(DisplayName = "Walking"),

    /** 
	 * Simplified walking on navigation data (e.g. navmesh). 
	 * If bGenerateOverlapEvents is true, then we will perform sweeps with each navmesh move.
	 * If bGenerateOverlapEvents is false then movement is cheaper but characters can overlap other objects without some extra process to repel/resolve their collisions.
	 */
    MOVE_NavWalking, // UMETA(DisplayName = "Navmesh Walking"),

    /** Falling under the effects of gravity, such as after jumping or walking off the edge of a surface. */
    MOVE_Falling,    // UMETA(DisplayName = "Falling"),

    /** Swimming through a fluid volume, under the effects of gravity and buoyancy. */
    MOVE_Swimming, //  UMETA(DisplayName = "Swimming"),

    /** Flying, ignoring the effects of gravity. Affected by the current physics volume's fluid friction. */
    MOVE_Flying, //     UMETA(DisplayName = "Flying"),

    /** User-defined custom movement mode, including many possible sub-modes. */
    MOVE_Custom,    // UMETA(DisplayName = "Custom"),

    MOVE_MAX ,     //  UMETA(Hidden),
};


