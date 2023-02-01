using Unity.Netcode.Components;
using UnityEngine;

namespace InkeepersKeep.Core.Utils
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
