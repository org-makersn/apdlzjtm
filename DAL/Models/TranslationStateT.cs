using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class TranslationStateT
    {
        public virtual string Gbn { get; set; }
        [IgnoreDataMember]
        public virtual int AllCnt { get; set; }
        [IgnoreDataMember]
        public virtual int DirectCnt { get; set; }
        [IgnoreDataMember]
        public virtual int RequestCnt { get; set; }
        [IgnoreDataMember]
        public virtual int CompleteCnt { get; set; }
        [IgnoreDataMember]
        public virtual int EnForKrCnt { get; set; }
        [IgnoreDataMember]
        public virtual int KrForEnCnt { get; set; }
    }
}
