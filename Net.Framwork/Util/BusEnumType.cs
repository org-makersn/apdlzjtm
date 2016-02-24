using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Net.Framework.Util
{
    /// <summary>
    /// 
    /// </summary>
    public enum BannerType
    {
        //[StringValue("00"), Description("전체")]
        [Description("전체")]
        All = 0,
        [Description("디자인")]
        Design = 1,
        [Description("프린팅")]
        Printing = 2,
        [Description("스토어")]
        Store = 3,
        [Description("메이커버스")]
        Bus = 4,
    }
}
