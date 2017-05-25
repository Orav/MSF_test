using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UE.Networking
{
    public interface INetworkPredictionInterface
    {

        //--------------------------------
        // Server hooks
        //--------------------------------

        /** (Server) Send position to client if necessary, or just ack good moves. */
        void SendClientAdjustment();  //              PURE_VIRTUAL(INetworkPredictionInterface::SendClientAdjustment,);

        /** (Server) Trigger a position update on clients, if the server hasn't heard from them in a while. */
        void ForcePositionUpdate(float DeltaTime);   // PURE_VIRTUAL(INetworkPredictionInterface::ForcePositionUpdate,);

        //--------------------------------
        // Client hooks
        //--------------------------------

        /** (Client) After receiving a network update of position, allow some custom smoothing, given the old transform before the correction and new transform from the update. */
        void SmoothCorrection(Vector3 OldLocation, Quaternion OldRotation, Vector3 NewLocation, Quaternion NewRotation); //PURE_VIRTUAL(INetworkPredictionInterface::SmoothCorrection,);

        //--------------------------------
        // Other
        //--------------------------------

        /** @return FNetworkPredictionData_Client instance used for network prediction. */
        FNetworkPredictionData_Client GetPredictionData_Client(); //const PURE_VIRTUAL(INetworkPredictionInterface::GetPredictionData_Client, return NULL;);

        /** @return FNetworkPredictionData_Server instance used for network prediction. */
        FNetworkPredictionData_Server GetPredictionData_Server(); // const PURE_VIRTUAL(INetworkPredictionInterface::GetPredictionData_Server, return NULL;);

        /** Accessor to check if there is already client data, without potentially allocating it on demand.*/
        bool HasPredictionData_Client(); //const PURE_VIRTUAL(INetworkPredictionInterface::HasPredictionData_Client, return false;);

        /** Accessor to check if there is already server data, without potentially allocating it on demand.*/
        bool HasPredictionData_Server(); //const PURE_VIRTUAL(INetworkPredictionInterface::HasPredictionData_Server, return false;);

        /** Resets client prediction data. */
        void ResetPredictionData_Client(); // PURE_VIRTUAL(INetworkPredictionInterface::ResetPredictionData_Client,);

        /** Resets server prediction data. */
        void ResetPredictionData_Server(); // PURE_VIRTUAL(INetworkPredictionInterface::ResetPredictionData_Server,);
    }


    // Network data representation on the client
    public class FNetworkPredictionData_Client
    {
        public FNetworkPredictionData_Client()
        {
        }

        //virtual ~FNetworkPredictionData_Client() { }
    }


    // Network data representation on the server
    public class FNetworkPredictionData_Server
    {
        // Server clock time when last server move was received or movement was forced to be processed
        float ServerTimeStamp;
        public FNetworkPredictionData_Server()

        {
            ServerTimeStamp = 0.0f;
        }

        //virtual ~FNetworkPredictionData_Server() { }


    }

}