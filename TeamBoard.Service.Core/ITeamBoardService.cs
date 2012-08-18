using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using TeamBoard.Commands;
using TeamBoard.Events;

namespace TeamBoard.Service.Core
{
    [ServiceContract(CallbackContract = typeof(ITeamBoardClient), SessionMode = SessionMode.Required)]
    [ServiceKnownType("GetKnownCommandTypes", typeof(DataContractHelper))]
    public interface ITeamBoardService
    {
        [OperationContract(IsOneWay = true)]
        void ProcessCommand(ICommand command);

        [OperationContract(IsOneWay = true)]
        void Ping();
    }

    [ServiceKnownType("GetKnownEventTypes", typeof(DataContractHelper))]
    public interface ITeamBoardClient
    {
        [OperationContract(IsOneWay = true)]
        void HandleEvent(IEvent @event);

        [OperationContract(IsOneWay = true)]
        void HandleHistory(IEvent[] events);

        [OperationContract(IsOneWay = true)]
        void PingBack();
    }

    static class DataContractHelper
    {
        public static IEnumerable<Type> GetKnownCommandTypes(ICustomAttributeProvider provider)
        {
            var interfaceType = typeof(ICommand);
            return interfaceType.Assembly.GetTypes().Where(x => interfaceType.IsAssignableFrom(x)).Union(new[] { typeof(TeamMemberRole) });
        }

        public static IEnumerable<Type> GetKnownEventTypes(ICustomAttributeProvider provider)
        {
            var interfaceType = typeof(IEvent);
            return interfaceType.Assembly.GetTypes().Where(x => interfaceType.IsAssignableFrom(x)).Union(new[] { typeof(TeamMemberRole) });
        }
    }

}
