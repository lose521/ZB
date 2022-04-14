using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.FrameWork.Cache
{
    public interface ICacheHelper
    {
        object Get(string key);
        object Get<T>(string key);
        object Remove(string key);
        void Store(string key, object value);
        void Store(string key, object value, DateTime expiresAt);
        void Store(string key, object value, TimeSpan expiresAt);
        void Update(string key, object value);
    }
}
