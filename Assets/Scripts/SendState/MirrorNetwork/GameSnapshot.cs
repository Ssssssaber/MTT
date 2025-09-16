using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SendState.MirrorNetwork
{
    public enum ObjectRepresentation
    {
        None = 0,
        MiniCube = 1,
        PlayerCube = 2,
        Camera = 3
    }

    public struct GameSnapshot
    {
        // alternative way: public byte[] WorldState;
        public ulong Tick;
        public List<ulong> IDs;
        public List<ObjectRepresentation> Types;
        public List<Vector3> Positions;
        public List<Quaternion> Rotations;
        public List<Color> Colors;

        public GameSnapshot(ulong tick = 0)
        {
            Tick = tick;
            IDs = new List<ulong>();
            Types = new List<ObjectRepresentation>();
            Positions = new List<Vector3>();
            Rotations = new List<Quaternion>();
            Colors = new List<Color>();
        }
    }

    public struct GameSnapshotMessage : NetworkMessage
    {
        public ulong Tick;
        public List<ulong> IDs;
        public List<ObjectRepresentation> Types;
        public List<Vector3> Positions;
        public List<Quaternion> Rotations;
        public List<Color> Colors;
    }
}