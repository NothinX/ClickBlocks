using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ClickBlocksServer
{
    public class ClickBlocksService : IClickBlocksService
    {
        public List<Record> GetPeopleList()
        {
            using (var entity = new DatabaseEntities())
            {
                List<Record> res = new List<Record>();
                entity.UserTable.ToList().ForEach(x =>
                {
                    res.Add(new Record() { UserName = x.UserName, Score = x.UserPoints });
                });
                return res;
            }
        }

        public List<Record> GetRecordList(string GameMode, string UserName = null)
        {
            using (var entity = new DatabaseEntities())
            {
                List<Record> res = new List<Record>();
                if (UserName != null)
                {
                    int uid = entity.UserTable.Where(x => x.UserName == UserName).First().UserId;
                    var q = entity.RecordTable.Where(x => x.UserId == uid && x.GameMode == GameMode).ToList();
                    q.ForEach(x => res.Add(new Record() { UserName = UserName, Score = x.GameScore }));
                }
                else
                {
                    var q = entity.RecordTable.Where(x => x.GameMode == GameMode).ToList();
                    q.ForEach(x =>
                    {
                        var name = entity.UserTable.Where(y => y.UserId == x.UserId).First().UserName;
                        res.Add(new Record() { UserName = name, Score = x.GameScore });
                    });
                }
                return res;
            }
        }

        public int Login(string UserName, string Password)
        {
            using (var entity = new DatabaseEntities())
            {
                var q = entity.UserTable.Where(x => x.UserName == UserName);
                if (q.Count() <= 0) return -1;
                else
                {
                    if (q.First().UserPassword != Password) return -1;
                    else return q.First().UserPoints;
                }
            }
        }

        public bool Register(string UserName, string Password)
        {
            using (var entity = new DatabaseEntities())
            {
                var q = entity.UserTable.Where(x => x.UserName == UserName);
                if (q.Count() > 0) return false;
                else
                {
                    int id = 1;
                    if (entity.UserTable.Count() > 0)
                    {
                        var qq = entity.UserTable.OrderBy(x => x.UserId);
                        id = qq.ToList().Last().UserId + 1;
                    }
                    entity.UserTable.Add(new UserTable() { UserId = id, UserName = UserName, UserPassword = Password, UserPoints = 0 });
                    entity.SaveChanges();
                    return true;
                }
            }
        }

        public void UploadPoints(string UserName, int UserPoints)
        {
            using (var entity = new DatabaseEntities())
            {
                var q = entity.UserTable.Where(x => x.UserName == UserName);
                q.First().UserPoints = UserPoints;
                entity.SaveChanges();
            }
        }

        public void UploadScore(string UserName, string GameMode, int GameScore, DateTime GameTime)
        {
            using (var entity = new DatabaseEntities())
            {
                int id = 1;
                if (entity.RecordTable.Count() > 0)
                {
                    var qq = entity.RecordTable.OrderBy(x => x.RecordId);
                    id = qq.ToList().Last().RecordId + 1;
                }
                int uid = entity.UserTable.Where(x => x.UserName == UserName).First().UserId;
                entity.RecordTable.Add(new RecordTable() { RecordId = id, UserId = uid, GameMode = GameMode, GameScore = GameScore, GameTime = GameTime });
                entity.SaveChanges();
            }
        }
    }
}
