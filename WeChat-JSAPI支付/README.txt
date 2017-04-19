请移步到个人博客查看详细教程-------http://www.cnblogs.com/longm/p/6732759.html
特别注意----JSAPI只能在微信环境下 测试！
配置：微信公众号中 添加微信支付配置

=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public const string APPID = AuthConnect.WeChat_AppId;
        public const string MCHID = "0000000";
        public const string KEY = AuthConnect.WeChat_AppPayKey;
        public const string APPSECRET = AuthConnect.WeChat_AppKey;


	C# 服务端-:
	UnifiedOrder order = new UnifiedOrder();
        order.appid = AppId;
        order.mch_id = partnerId;
        order.nonce_str = TenpayUtil.getNoncestr();
        order.body = body;
        order.out_trade_no = orderNumber;
        
        order.total_fee = Price;
        order.spbill_create_ip = Page.Request.UserHostAddress;
        order.notify_url = "Notify.aspx";
        order.trade_type = "JSAPI";
        if (ViewState["OpenID"] != null)
        {
            order.openid = ViewState["OpenID"].ToString();  //JSAPI必须传入openid
        }
        
        TenpayUtil tu = new TenpayUtil();
        PrepayId = tu.getPrepay_id(order, key);
        string package = "prepay_id=" + PrepayId;
        NonceStr = order.nonce_str;
        TimeStamp = TenpayUtil.getTimestamp();

        SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
        sParams.Add("appId", AppId);
        sParams.Add("nonceStr", NonceStr);
        sParams.Add("package", package);
        sParams.Add("signType", "MD5");
        sParams.Add("timeStamp", TimeStamp);
        Sign = tu.getsign(sParams, key);
        Json = "{\"code\":\"1\",\"data\":{\"appId\":\"" + AppId + "\",\"nonceStr\":\"" + NonceStr + "\",\"package\":\"" + package + "\",\"timeStamp\":\"" + TimeStamp + "\",\"signType\":\"MD5\",\"paySign\":\"" + Sign + "\"}}";
		
	前端JS---：
	$.post("/Pay/PayApi.aspx", ret, function (result) {
		var result = JSON.parse(result); //后台获取到的参数
		if (result.code == 1) {
			WeixinJSBridge.invoke('getBrandWCPayRequest', result.data,
                            function (res) {
                                // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。
                                switch (res.errMsg) {
                                case "get_brand_wcpay_request:ok":
                                    alert("支付成功");
                                    break;
                                case "get_brand_wcpay_request:cancel":
                                    alert("支付取消");
                                    break;
                                default:
                                    alert("支付失败");
                                    break;
                                }
                            }
                        );
		}
	}