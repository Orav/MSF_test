using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UE.Networking
{
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

        MOVE_MAX,     //  UMETA(Hidden),
    };

    [Flags]
    public enum SavedMoveFlags
    {
        bPressedJump = 0,
        bWantsToCrouch = 1,
        bForceMaxAccel = 2,
        /** If true, can't combine this move with another move. */
        bForceNoCombine = 4,
        /** If true this move is using an old TimeStamp, before a reset occurred. */
        bOldTimeStampBeforeReset = 8,

    }
    // Bit masks used by GetCompressedFlags() to encode movement information.
    [Flags]
    public enum CompressedFlags
    {
        FLAG_JumpPressed = 0x01,    // Jump pressed
        FLAG_WantsToCrouch = 0x02,  // Wants to crouch
        FLAG_Reserved_1 = 0x04, // Reserved for future use
        FLAG_Reserved_2 = 0x08, // Reserved for future use
                                // Remaining bit masks are available for custom flags.
        FLAG_Custom_0 = 0x10,
        FLAG_Custom_1 = 0x20,
        FLAG_Custom_2 = 0x40,
        FLAG_Custom_3 = 0x80,
    }
}