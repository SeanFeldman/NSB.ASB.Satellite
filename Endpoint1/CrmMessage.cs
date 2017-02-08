using System.Collections.Generic;
using NServiceBus;

public class OwningExtension
{
}

public class ParentContext
{
}

public class SharedVariable
{
    public string key { get; set; }
    public bool value { get; set; }
}

public class Body
{
    public string BusinessUnitId { get; set; }
    public string CorrelationId { get; set; }
    public int Depth { get; set; }
    public string InitiatingUserId { get; set; }
    public List<object> InputParameters { get; set; }
    public bool IsExecutingOffline { get; set; }
    public bool IsInTransaction { get; set; }
    public bool IsOfflinePlayback { get; set; }
    public int IsolationMode { get; set; }
    public string MessageName { get; set; }
    public int Mode { get; set; }
    public string OperationCreatedOn { get; set; }
    public string OperationId { get; set; }
    public string OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public List<object> OutputParameters { get; set; }
    public OwningExtension OwningExtension { get; set; }
    public ParentContext ParentContext { get; set; }
    public List<object> PostEntityImages { get; set; }
    public List<object> PreEntityImages { get; set; }
    public string PrimaryEntityId { get; set; }
    public string PrimaryEntityName { get; set; }
    public string RequestId { get; set; }
    public string SecondaryEntityName { get; set; }
    public List<SharedVariable> SharedVariables { get; set; }
    public int Stage { get; set; }
    public string UserId { get; set; }
}

public class CrmMessage : IMessage
{
    public Body body { get; set; }
    public string contentType { get; set; }
    public string correlationId { get; set; }
    public object deadLetterSource { get; set; }
    public int deliveryCount { get; set; }
    public int enqueuedSequenceNumber { get; set; }
    public string enqueuedTimeUtc { get; set; }
    public string expiresAtUtc { get; set; }
    public bool forcePersistence { get; set; }
    public bool isBodyConsumed { get; set; }
    public object label { get; set; }
    public object lockedUntilUtc { get; set; }
    public object lockToken { get; set; }
    public string messageId { get; set; }
    public object partitionKey { get; set; }
    public object replyTo { get; set; }
    public object replyToSessionId { get; set; }
    public string scheduledEnqueueTimeUtc { get; set; }
    public int sequenceNumber { get; set; }
    public object sessionId { get; set; }
    public int size { get; set; }
    public int state { get; set; }
    public string timeToLive { get; set; }
    public object to { get; set; }
    public object viaPartitionKey { get; set; }
}
