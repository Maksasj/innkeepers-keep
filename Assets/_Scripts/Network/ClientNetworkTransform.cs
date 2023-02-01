using Unity.Netcode.Components;

namespace InkeepersKeep.Core.Network
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
