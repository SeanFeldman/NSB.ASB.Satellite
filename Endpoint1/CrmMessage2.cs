using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Endpoint1
{
    public class Attribute
    {
        public string key { get; set; }
        public object value { get; set; }
    }

    public class FormattedValue
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class Value
    {
        public string __type { get; set; }
        public List<Attribute> Attributes { get; set; }
        public object EntityState { get; set; }
        public List<FormattedValue> FormattedValues { get; set; }
        public string Id { get; set; }
        public List<object> KeyAttributes { get; set; }
        public string LogicalName { get; set; }
        public List<object> RelatedEntities { get; set; }
        public object RowVersion { get; set; }
    }

    public class InputParameter
    {
        public string key { get; set; }
        public Value value { get; set; }
    }

    public class OutputParameter
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class OwningExtension
    {
        public string Id { get; set; }
        public List<object> KeyAttributes { get; set; }
        public string LogicalName { get; set; }
        public object Name { get; set; }
        public object RowVersion { get; set; }
    }

    public class InputParameter2
    {
        public string key { get; set; }
        public object value { get; set; }
    }

    public class OwningExtension2
    {
        public string Id { get; set; }
        public List<object> KeyAttributes { get; set; }
        public string LogicalName { get; set; }
        public object Name { get; set; }
        public object RowVersion { get; set; }
    }

    public class SharedVariable
    {
        public string key { get; set; }
        public object value { get; set; }
    }

    public class ParentContext
    {
        public string BusinessUnitId { get; set; }
        public string CorrelationId { get; set; }
        public int Depth { get; set; }
        public string InitiatingUserId { get; set; }
        public List<InputParameter2> InputParameters { get; set; }
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
        public OwningExtension2 OwningExtension { get; set; }
       // public object ParentContext { get; set; }
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

    public class Attribute2
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class Value2
    {
        public List<Attribute2> Attributes { get; set; }
        public object EntityState { get; set; }
        public List<object> FormattedValues { get; set; }
        public string Id { get; set; }
        public List<object> KeyAttributes { get; set; }
        public string LogicalName { get; set; }
        public List<object> RelatedEntities { get; set; }
        public object RowVersion { get; set; }
    }

    public class PostEntityImage
    {
        public string key { get; set; }
        public Value2 value { get; set; }
    }

    public class SharedVariable2
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
        public List<InputParameter> InputParameters { get; set; }
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
        public List<OutputParameter> OutputParameters { get; set; }
        public OwningExtension OwningExtension { get; set; }
        public ParentContext ParentContext { get; set; }
        public List<PostEntityImage> PostEntityImages { get; set; }
        public List<object> PreEntityImages { get; set; }
        public string PrimaryEntityId { get; set; }
        public string PrimaryEntityName { get; set; }
        public string RequestId { get; set; }
        public string SecondaryEntityName { get; set; }
        public List<SharedVariable2> SharedVariables { get; set; }
        public int Stage { get; set; }
        public string UserId { get; set; }
    }

    public class Properties
    {
        /*public string invalid_name__http://schemas.microsoft.com/xrm/2011/Claims/Organization { get; set; }
    public object __invalid_name__http://schemas.microsoft.com/xrm/2011/Claims/User { get; set; }
    public object __invalid_name__http://schemas.microsoft.com/xrm/2011/Claims/InitiatingUser { get; set; }
    public string __invalid_name__http://schemas.microsoft.com/xrm/2011/Claims/EntityLogicalName { get; set; }
    public string __invalid_name__http://schemas.microsoft.com/xrm/2011/Claims/RequestName { get; set; }
    */
    public string singularityheader { get; set; }
    }

    public class CrmMessage2:IMessage
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
        public Properties properties { get; set; }
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

    }
