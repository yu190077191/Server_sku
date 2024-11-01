using System;
using System.Text;

namespace WF.Model
{
    public class DataResult
    {

        private int _status = 1;
        /// <summary>
        /// 请求状态（-1验证失败；-2参数错误；-3请求失败；-4数据异常；1请求成功）
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        private string _message = "请求成功";
        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            set { _message = value; }
            get { return _message; }
        }

        private object _data;
        /// <summary>
        /// 
        /// </summary>
        public object Data
        {
            set { _data = value; }
            get { return _data; }
        }
    }
}
