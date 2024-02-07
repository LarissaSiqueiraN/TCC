using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DTOs
{
    public class ResponseDto<T>
    {
        private List<string> _lstErros { get; set; } = new List<string>();
        private T _objResponse { get; set; }

        public bool hasErrors { get { return _lstErros != null && _lstErros.Count > 0 ? true : false; } }

        public ResponseDto<T> addError(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
                if (!_lstErros.Any(x => x.ToLower().Trim() == msg.ToLower().Trim()))
                    _lstErros.Add(msg);

            return this;
        }
        public ResponseDto<T> addError(Exception ex)
        {

            if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
            {
                if (ex.Message.ToLower().IndexOf("[show]") != -1)
                    addError(ex.Message.Replace("[show]", ""));
            }
            return this;
        }
        public List<string> getErrors { get { return _lstErros; } }
        public ResponseDto<T> setResponse(T obj)
        {
            _objResponse = obj;
            return this;
        }
        public T getResponse { get { return _objResponse; } }



    }
}
