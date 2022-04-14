using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.Common.Entity
{

    public class FileEntity
    {
        //        name 文件名 string	-
        //percent 上传进度    number	-
        //status 上传状态，不同状态展示颜色也会有所不同 error | success | done | uploading | removed	-
        //thumbUrl 缩略图地址   string	-
        //uid 唯一标识符，不设置时会自动生成 string	-
        //url
        public string name { get; set; }
        public string percent { get; set; }
        public status status { get; set; }
        public string thumbUrl { get; set; }
        public string uid { get; set; }
        public string url { get; set; }
    }

    public enum status
    {
        error,success ,done ,uploading ,removed
    }
}
