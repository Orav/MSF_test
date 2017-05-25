using System;
using System.Collections;
using System.Linq;
using Barebones.MasterServer;
using Barebones.Utils;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UE.Networking
{
    [RequireComponent(typeof(CharacterController))]
    //[RequireComponent(typeof(AudioSource))]
    [NetworkSettings(channel = 0, sendInterval = 0.02f)]
    public class UECharacterMovementController : NetworkBehaviour
    {
        [SerializeField]
        public float NetworkNoSmoothUpdateDistance { get; set; }
        [SerializeField]
        public float NetworkMaxSmoothUpdateDistance { get; set; }

        private void Awake()
        {

        }

        private void FixedUpdate()
        {

        }

        public override void OnStartAuthority()
        {

        }

        public override void OnStartClient()
        {

        }

        public override void OnStartLocalPlayer()
        {

        }

        public override void OnStartServer()
        {

        }

        public override void OnStopAuthority()
        {

        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}