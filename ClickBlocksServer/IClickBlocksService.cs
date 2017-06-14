using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ClickBlocksServer
{
    [ServiceContract(Namespace = "ClickBlocks")]
    public interface IClickBlocksService
    {
        [OperationContract]
        int Login(string UserName, string Password);
        [OperationContract]
        bool Register(string UserName, string Password);
        [OperationContract]
        void UploadPoints(string UserName, int UserPoints);
        [OperationContract]
        void UploadScore(string UserName, string GameMode, int GameScore, DateTime GameTime);
        [OperationContract]
        List<Record> GetRecordList(string GameMode, string UserName = null);
        [OperationContract]
        List<Record> GetPeopleList();
    }

    [DataContract]
    public class Record
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int Score { get; set; }
    }

}
