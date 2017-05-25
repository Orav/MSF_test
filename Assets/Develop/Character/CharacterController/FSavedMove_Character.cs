using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UE.Networking
{
    /** FSavedMove_Character represents a saved move on the client that has been sent to the server and might need to be played back. */
    public class FSavedMove_Character
    {

        public FSavedMove_Character() { }
        // ~FSavedMove_Character();

        //   uint32 bPressedJump:1;
        //uint32 bWantsToCrouch:1;
        //uint32 bForceMaxAccel:1;

        /** If true, can't combine this move with another move. */
        //uint32 bForceNoCombine:1;

        /** If true this move is using an old TimeStamp, before a reset occurred. */
        //uint32 bOldTimeStampBeforeReset:1;

        float TimeStamp;    // Time of this move.
        float DeltaTime;    // amount of time for this move
        float CustomTimeDilation;
        float JumpKeyHoldTime;
        Int32 JumpMaxCount;
        Int32 JumpCurrentCount;
        EMovementMode MovementMode; // packed movement mode

        // Information at the start of the move
        Vector3 StartLocation;
        Vector3 StartRelativeLocation;
        Vector3 StartVelocity;
        FFindFloorResult StartFloor;

        Quaternion StartRotation; //FRotator StartRotation
        Quaternion StartControlRotation;    //FRotator StartControlRotation;
        Quaternion StartBaseRotation;    // rotation of the base component (or bone), only saved if it can move.
        float StartCapsuleRadius;
        float StartCapsuleHalfHeight;
        Collider StartBase; //TWeakObjectPtr<UPrimitiveComponent> StartBase;
        string StartBoneName;

        // Information after the move has been performed
        Vector3 SavedLocation;
        Quaternion SavedRotation;  //FRotator SavedRotation;
        Vector3 SavedVelocity;
        Vector3 SavedRelativeLocation;
        Quaternion SavedControlRotation; // FRotator SavedControlRotation;
        Collider EndBase;         //TWeakObjectPtr<UPrimitiveComponent> EndBase;
        string EndBoneName;

        Vector3 Acceleration;

        // Cached to speed up iteration over IsImportantMove().
        Vector3 AccelNormal;
        float AccelMag;

        //TWeakObjectPtr<class UAnimMontage> RootMotionMontage;
        Animator RootMotionMontage;
        float RootMotionTrackPosition;

        // TODO:
        //FRootMotionMovementParams RootMotionMovement;
        // TODO:
        //FRootMotionSourceGroup SavedRootMotion;

        /** Threshold for deciding this is an "important" move based on DP with last acked acceleration. */
        float AccelDotThreshold;
        /** Threshold for deciding is this is an important move because acceleration magnitude has changed too much */
        float AccelMagThreshold;
        /** Threshold for deciding if we can combine two moves, true if cosine of angle between them is <= this. */
        float AccelDotThresholdCombine;

        /** Clear saved move properties, so it can be re-used. */
        public void Clear() { }

        /** Called to set up this saved move (when initially created) to make a predictive correction. */
        public virtual void SetMoveFor(GameObject Character, float InDeltaTime, Vector3 NewAccel, FNetworkPredictionData_Client_Character ClientData)
        { }
        /** Set the properties describing the position, etc. of the moved pawn at the start of the move. */
        public virtual void SetInitialPosition(GameObject Character) { }

        /** @Return true if this move is an "important" move that should be sent again if not acked by the server */
        public virtual bool IsImportantMove(ref FSavedMove_Character LastAckedMove) { }

        /** Returns starting position if we were to revert the move, either absolute StartLocation,
         *  or StartRelativeLocation offset from MovementBase's current location (since we want to try to move forward at this time). */
        public virtual Vector3 GetRevertedLocation() { }

        enum EPostUpdateMode
        {
            PostUpdate_Record,      // Record a move after having run the simulation
            PostUpdate_Replay,      // Update after replaying a move for a client correction
        }

        /** Set the properties describing the final position, etc. of the moved pawn. */
        public virtual void PostUpdate(ACharacter* C, EPostUpdateMode PostUpdateMode) { }

        /** @Return true if this move can be combined with NewMove for replication without changing any behavior */
        public virtual bool CanCombineWith(FSavedMove NewMove, ACharacter* InPawn, float MaxDelta) { }

    /** Called before ClientUpdatePosition uses this SavedMove to make a predictive correction	 */
    public virtual void PrepMoveFor(ACharacter* C) { }

    /** @returns a byte containing encoded special movement information (jumping, crouching, etc.)	 */
    public virtual CompressedFlags GetCompressedFlags() { }

}
}   
