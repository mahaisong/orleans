using System;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Orleans.Streams
{
    // This is the extension interface for stream consumers
    internal interface IStreamConsumerExtension : IGrainExtension
    {
        Task<StreamHandshakeToken> DeliverImmutable(GuidId subscriptionId, QualifiedStreamId streamId, Immutable<object> item, StreamSequenceToken currentToken, StreamHandshakeToken handshakeToken);
        Task<StreamHandshakeToken> DeliverMutable(GuidId subscriptionId, QualifiedStreamId streamId, object item, StreamSequenceToken currentToken, StreamHandshakeToken handshakeToken);
        Task<StreamHandshakeToken> DeliverBatch(GuidId subscriptionId, QualifiedStreamId streamId, Immutable<IBatchContainer> item, StreamHandshakeToken handshakeToken);
        Task CompleteStream(GuidId subscriptionId);
        Task ErrorInStream(GuidId subscriptionId, Exception exc);
        Task<StreamHandshakeToken> GetSequenceToken(GuidId subscriptionId);
    }

    // This is the extension interface for stream producers
    internal interface IStreamProducerExtension : IGrainExtension
    {
        [AlwaysInterleave]
        Task AddSubscriber(GuidId subscriptionId, QualifiedStreamId streamId, IStreamConsumerExtension streamConsumer, string filterData);

        [AlwaysInterleave]
        Task RemoveSubscriber(GuidId subscriptionId, QualifiedStreamId streamId);
    }
}
