using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace UE.Networking
{
    public class FNetworkPredictionData_Client_Character : FNetworkPredictionData_Client
    {
        public FNetworkPredictionData_Client_Character(UECharacterMovementController ClientMovement)
        {
            ClientUpdateTime = 0.0f;
	        CurrentTimeStamp = 0.0f;
            PendingMove = null;
	        LastAckedMove = null;
            MaxFreeMoveCount = 96;
	        MaxSavedMoveCount = 96;
            bUpdatePosition = false;
            //bSmoothNetUpdates = false; // Deprecated
            OriginalMeshTranslationOffset = Vector3.zero; // (ForceInitToZero)
            MeshTranslationOffset = Vector3.zero;
            OriginalMeshRotationOffset = Quaternion.identity;//(FQuat::Identity)
	        MeshRotationOffset = Quaternion.identity;//(FQuat::Identity)
            MeshRotationTarget = Quaternion.identity;//(FQuat::Identity)
	        LastCorrectionDelta = 0.0f;
	        LastCorrectionTime = 0.0f;
	        SmoothingServerTimeStamp = 0.0f;
	        SmoothingClientTimeStamp = 0.0f;
	// CurrentSmoothTime(0.f) // Deprecated
	 //bUseLinearSmoothing = false; // Deprecated
            MaxSmoothNetUpdateDist = 0.0f;
            NoSmoothNetUpdateDist = 0.0f;
            SmoothNetUpdateTime = 0.0f;
            SmoothNetUpdateRotationTime = 0.0f;
            //MaxResponseTime(0.125f) // Deprecated, use MaxMoveDeltaTime instead
            MaxMoveDeltaTime = 0.125f;
            LastSmoothLocation = Vector3.zero;
            LastServerLocation = Vector3.zero;
	        SimulatedDebugDrawTime = 0.0f;
            {
                MaxSmoothNetUpdateDist = ClientMovement.NetworkMaxSmoothUpdateDistance;
                NoSmoothNetUpdateDist = ClientMovement.NetworkNoSmoothUpdateDistance;

                const bool bIsListenServer = (ClientMovement.isServer  == NM_ListenServer);
                SmoothNetUpdateTime = (bIsListenServer ? ClientMovement.ListenServerNetworkSimulatedSmoothLocationTime : ClientMovement.NetworkSimulatedSmoothLocationTime);
                SmoothNetUpdateRotationTime = (bIsListenServer ? ClientMovement.ListenServerNetworkSimulatedSmoothRotationTime : ClientMovement.NetworkSimulatedSmoothRotationTime);


                NetworkManager GameNetworkManager =
                AGameNetworkManager* GameNetworkManager = AGameNetworkManager::StaticClass()->GetDefaultObject<AGameNetworkManager>();
                if (GameNetworkManager)
                {
                    MaxMoveDeltaTime = GameNetworkManager->MaxMoveDeltaTime;  // =0.125f
                }

                MaxResponseTime = MaxMoveDeltaTime; // MaxResponseTime is deprecated, use MaxMoveDeltaTime instead

                if (ClientMovement.GetOwnerRole() == ROLE_AutonomousProxy)
                {
                    SavedMoves.Reserve(MaxSavedMoveCount);
                    FreeMoves.Reserve(MaxFreeMoveCount);
                }
            }
        }
        // ~FNetworkPredictionData_Client_Character() { }

        /** Client timestamp of last time it sent a servermove() to the server.  Used for holding off on sending movement updates to save bandwidth. */
        public float ClientUpdateTime;

        /** Current TimeStamp for sending new Moves to the Server. */
        public float CurrentTimeStamp;

        public List<FSavedMove_Character> SavedMoves;       // Buffered moves pending position updates, orderd oldest to newest. Moves that have been acked by the server are removed.
        public List<FSavedMove_Character> FreeMoves;        // freed moves, available for buffering
        public FSavedMove_Character PendingMove;              // PendingMove already processed on client - waiting to combine with next movement to reduce client to server bandwidth
        public FSavedMove_Character LastAckedMove;            // Last acknowledged sent move.

        public Int32 MaxFreeMoveCount;                 // Limit on size of free list
        public Int32 MaxSavedMoveCount;                // Limit on the size of the saved move buffer

        /** RootMotion saved while animation is updated, so we can store it and replay if needed in case of a position correction. */
        public FRootMotionMovementParams RootMotionMovement;

        public bool bUpdatePosition; // when true, update the position (via ClientUpdatePosition)

    // Mesh smoothing variables (for network smoothing)
    //
    /** Whether to smoothly interpolate pawn position corrections on clients based on received location updates */
    //DEPRECATED(4.11, "bSmoothNetUpdates will be removed, use UCharacterMovementComponent::NetworkSmoothingMode instead.")

    //uint32 bSmoothNetUpdates:1;

        /** Used for position smoothing in net games */
        public Vector3 OriginalMeshTranslationOffset;

        /** World space offset of the mesh. Target value is zero offset. Used for position smoothing in net games. */
        public Vector3 MeshTranslationOffset;

        /** Used for rotation smoothing in net games (only used by linear smoothing). */
        public Quaternion OriginalMeshRotationOffset;

        /** Component space offset of the mesh. Used for rotation smoothing in net games. */
        public Quaternion MeshRotationOffset;

        /** Target for mesh rotation interpolation. */
        public Quaternion MeshRotationTarget;

        /** Used for remembering how much time has passed between server corrections */
        float LastCorrectionDelta;

        /** Used to track time of last correction */
        float LastCorrectionTime;

        /** Used to track the timestamp of the last server move. */
        double SmoothingServerTimeStamp;

        /** Used to track the client time as we try to match the server.*/
        double SmoothingClientTimeStamp;

    /** Used to track how much time has elapsed since last correction. It can be computed as World->TimeSince(LastCorrectionTime). */
    //DEPRECATED(4.11, "CurrentSmoothTime will be removed, use LastCorrectionTime instead.")

    //float CurrentSmoothTime;

    /** Used to signify that linear smoothing is desired */
    //DEPRECATED(4.11, "bUseLinearSmoothing will be removed, use UCharacterMovementComponent::NetworkSmoothingMode instead.")

    //bool bUseLinearSmoothing;

        /**
         * Copied value from UCharacterMovementComponent::NetworkMaxSmoothUpdateDistance.
         * @see UCharacterMovementComponent::NetworkMaxSmoothUpdateDistance
         */
        float MaxSmoothNetUpdateDist;

        /**
         * Copied value from UCharacterMovementComponent::NetworkNoSmoothUpdateDistance.
         * @see UCharacterMovementComponent::NetworkNoSmoothUpdateDistance
         */
        float NoSmoothNetUpdateDist;

        /** How long to take to smoothly interpolate from the old pawn position on the client to the corrected one sent by the server.  Must be >= 0. Not used for linear smoothing. */
        float SmoothNetUpdateTime;

        /** How long to take to smoothly interpolate from the old pawn rotation on the client to the corrected one sent by the server.  Must be >= 0. Not used for linear smoothing. */
        float SmoothNetUpdateRotationTime;

    /** (DEPRECATED) How long server will wait for client move update before setting position */
    //DEPRECATED(4.12, "MaxResponseTime has been renamed to MaxMoveDeltaTime for clarity in what it does and will be removed, use MaxMoveDeltaTime instead.")

    //float MaxResponseTime;

        /** 
         * Max delta time for a given move, in real seconds
         * Based off of AGameNetworkManager::MaxMoveDeltaTime config setting, but can be modified per actor
         * if needed.
         * This value is mirrored in FNetworkPredictionData_Server, which is what server logic runs off of.
         * Client needs to know this in order to not send move deltas that are going to get clamped anyway (meaning
         * they'll be rejected/corrected).
         * Note: This was previously named MaxResponseTime, but has been renamed to reflect what it does more accurately
         */
        float MaxMoveDeltaTime;

        /** Values used for visualization and debugging of simulated net corrections */
        Vector3 LastSmoothLocation;
        Vector3 LastServerLocation;
        float SimulatedDebugDrawTime;

        /** Array of replay samples that we use to interpolate between to get smooth location/rotation/velocity/ect */
        public List<FCharacterReplaySample> ReplaySamples;

        /** Finds SavedMove index for given TimeStamp. Returns INDEX_NONE if not found (move has been already Acked or cleared). */
        public Int32 GetSavedMoveIndex(float TimeStamp) { }

        /** Ack a given move. This move will become LastAckedMove, SavedMoves will be adjusted to only contain unAcked moves. */
        public void AckMove(Int32 AckedMoveIndex) { }

        /** Allocate a new saved move. Subclasses should override this if they want to use a custom move class. */
        public virtual FSavedMove_Character AllocateNewMove() { }

        /** Return a move to the free move pool. Assumes that 'Move' will no longer be referenced by anything but possibly the FreeMoves list. Clears PendingMove if 'Move' is PendingMove. */
        public virtual void FreeMove(FSavedMove_Character Move);

        /** Tries to pull a pooled move off the free move list, otherwise allocates a new move. Returns NULL if the limit on saves moves is hit. */
        public virtual FSavedMove_Character CreateSavedMove() { }

        /** Update CurentTimeStamp from passed in DeltaTime.
            It will measure the accuracy between passed in DeltaTime and how Server will calculate its DeltaTime.
            If inaccuracy is too high, it will reset CurrentTimeStamp to maintain a high level of accuracy.
            @return DeltaTime to use for Client's physics simulation prior to replicate move to server. */
        public float UpdateTimeStampAndDeltaTime(float DeltaTime, ACharacter & CharacterOwner, class UCharacterMovementComponent & CharacterMovementComponent);

    }
}
