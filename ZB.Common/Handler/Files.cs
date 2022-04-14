using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.Common.Entity;

namespace ZB.Common.Handler
{
    public class Files
    {
        public string url;
        public Files(string url)
        {
            this.url = url;
        }

        public FileEntity GetFile()
        {
            FileEntity file = new FileEntity();
            file.name = Path.GetFileName(url);
            file.status = status.done;
            file.url = Config.AppAddress + "/api/Upload/GetImage?url=" + this.url;
            return file;
        }
    }
}
