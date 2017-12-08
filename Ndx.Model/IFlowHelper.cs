using System;

namespace Ndx.Model
{
    [Flags]
    public enum FlowFlags { None = 0, StartNewConversation = 1, TcpFin = 2, TcpSyn = 4}

    /// <summary>
    /// Defines an interface necessary for providing flow related information.
    /// </summary>
    /// <typeparam name="TFrame"></typeparam>
    public interface IFlowHelper<TFrame>
    {
        (FlowKey, FlowFlags) GetFlowKey(TFrame packet);
        long UpdateConversation(TFrame packet, FlowAttributes flowAttributes);
    }
}