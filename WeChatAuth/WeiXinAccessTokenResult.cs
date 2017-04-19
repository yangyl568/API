using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class WeiXinAccessTokenResult
{
    public WeiXinAccessTokenModel SuccessResult { get; set; }
    public bool Result { get; set; }

    public WeiXinHelper.WeiXinErrorMsg ErrorResult { get; set; }
}