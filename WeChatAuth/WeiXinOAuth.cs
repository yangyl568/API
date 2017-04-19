using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

public class WeiXinOAuth
{
    /// <summary>
    /// 获取微信Code
    /// </summary>
    /// <param name="appId">微信AppId</param>
    /// <param name="appSecret">微信AppSecret</param>
    /// <param name="redirectUrl">返回的登录地址，要进行Server.Un编码</param>
    /// <param name="isWap">true=微信内部登录 false=pc网页登录</param>
    public string GetWeiXinCode(string appId, string appSecret, string redirectUrl, bool isWap)
    {
        var r = new Random();
        //微信登录授权
        //string url = "https://open.weixin.qq.com/connect/qrconnect?appid=" + appId + "&redirect_uri=" + redirectUrl +"&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
        //微信OpenId授权
        //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appId + "&redirect_uri=" + redirectUrl +"&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
        //微信用户信息授权
        var url = "";
        if (isWap)
        {
            url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appId + "&redirect_uri=" +
                  redirectUrl + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
        }
        else
        {
            url = "https://open.weixin.qq.com/connect/qrconnect?appid=" + appId + "&redirect_uri=" + redirectUrl +
                  "&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
        }
        return url;
    }
    /// <summary>
    /// 获取access_token
    /// </summary>
    /// <param name="appid">应用ID</param>
    /// <param name="secret">应用密钥</param>
    /// <returns>AccessToken实体</returns>
    public  WeiXinAccessTokenResult GetAccessToken(string appid, string secret)
    {
        var url =
        string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
        string jsonStr = string.Empty;
        HttpHelper.Send(url, "Get", out  jsonStr);
        var result = new WeiXinAccessTokenResult();
        if (jsonStr.Contains("errcode"))
        {
            var errorResult = JsonHelper.JsonDeserialize<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
            result.ErrorResult = errorResult;
            result.Result = false;
        }
        else
        {
            var model = JsonHelper.JsonDeserialize<WeiXinAccessTokenModel>(jsonStr);
            result.SuccessResult = model;
            result.Result = true;
        }
        return result;
    }
    /// <summary>
    /// 拉取用户信息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <returns></returns>
    public WeiXinHelper.WeiXinUserInfoResult GetWXUserInfo(string accessToken, string openId)
    {
        string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + accessToken + "&openid=" + openId + "&lang=zh_CN";

        string jsonStr = string.Empty;
        HttpHelper.Send(url, "Get", out  jsonStr);

        var result = new WeiXinHelper.WeiXinUserInfoResult();
        if (jsonStr.Contains("errcode"))
        {
            var errorResult = JsonHelper.JsonDeserialize<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
            result.ErrorMsg = errorResult;
            result.Result = false;
        }
        else
        {
            var userInfo = JsonHelper.JsonDeserialize<WeiXinHelper.WeiXinUserInfo>(jsonStr);
            result.UserInfo = userInfo;
            result.Result = true;
        }
        return result;
    }
    /// <summary>
    /// 通过code获取access_token
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="appSecret"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public WeiXinAccessTokenResult GetWeiXinAccessToken(string appId, string appSecret, string code)
    {
        string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appId + "&secret=" + appSecret +
          "&code=" + code + "&grant_type=authorization_code";

        string jsonStr = string.Empty;
        HttpHelper.Send(url, "Get", out  jsonStr);// UtilsHelper.GetHttp(url);

        var result = new WeiXinAccessTokenResult();
        if (jsonStr.Contains("errcode"))
        {
            var errorResult = JsonHelper.JsonDeserialize<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
            result.ErrorResult = errorResult;
            result.Result = false;
        }
        else
        {
            var model = JsonHelper.JsonDeserialize<WeiXinAccessTokenModel>(jsonStr);
            result.SuccessResult = model;
            result.Result = true;
        }
        return result;
    }
    /// <summary>
    /// 拉取用户信息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <returns></returns>
    public WeiXinHelper.WeiXinUserInfoResult GetWeiXinUserInfo(string accessToken, string openId)
    {
        string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + accessToken + "&openid=" + openId + "&lang=zh_CN";

        string jsonStr = string.Empty;
        HttpHelper.Send(url, "Get", out  jsonStr);// UtilsHelper.GetHttp(url);

        var result = new WeiXinHelper.WeiXinUserInfoResult();
        if (jsonStr.Contains("errcode"))
        {
            var errorResult = JsonHelper.JsonDeserialize<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
            result.ErrorMsg = errorResult;
            result.Result = false;
        }
        else
        {
            var userInfo = JsonHelper.JsonDeserialize<WeiXinHelper.WeiXinUserInfo>(jsonStr);
            result.UserInfo = userInfo;
            result.Result = true;
        }
        return result;
    }

}