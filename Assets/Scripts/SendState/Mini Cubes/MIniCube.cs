using SendState.MirrorNetwork;
using SendState.DI;
using UnityEngine;

namespace SendState.MiniCubes
{
    public class MiniCube : UpdateableBehaviour
    {
        public override ObjectRepresentation GetRepresentation()
        {
            return ObjectRepresentation.MiniCube;
        }
    }
}