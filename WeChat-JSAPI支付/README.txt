���Ʋ������˲��Ͳ鿴��ϸ�̳�-------http://www.cnblogs.com/longm/p/6732759.html
�ر�ע��----JSAPIֻ����΢�Ż����� ���ԣ�
���ã�΢�Ź��ں��� ���΢��֧������

=======��������Ϣ���á�=====================================
        /* ΢�Ź��ں���Ϣ����
        * APPID����֧����APPID���������ã�
        * MCHID���̻��ţ��������ã�
        * KEY���̻�֧����Կ���ο������ʼ����ã��������ã�
        * APPSECRET�������ʺ�secert����JSAPI֧����ʱ����Ҫ���ã�
        */
        public const string APPID = AuthConnect.WeChat_AppId;
        public const string MCHID = "0000000";
        public const string KEY = AuthConnect.WeChat_AppPayKey;
        public const string APPSECRET = AuthConnect.WeChat_AppKey;


	C# �����-:
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
            order.openid = ViewState["OpenID"].ToString();  //JSAPI���봫��openid
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
		
	ǰ��JS---��
	$.post("/Pay/PayApi.aspx", ret, function (result) {
		var result = JSON.parse(result); //��̨��ȡ���Ĳ���
		if (result.code == 1) {
			WeixinJSBridge.invoke('getBrandWCPayRequest', result.data,
                            function (res) {
                                // ʹ�����Ϸ�ʽ�ж�ǰ�˷���,΢���Ŷ�֣����ʾ��res.err_msg�����û�֧���ɹ��󷵻�    ok����������֤�����Կɿ���
                                switch (res.errMsg) {
                                case "get_brand_wcpay_request:ok":
                                    alert("֧���ɹ�");
                                    break;
                                case "get_brand_wcpay_request:cancel":
                                    alert("֧��ȡ��");
                                    break;
                                default:
                                    alert("֧��ʧ��");
                                    break;
                                }
                            }
                        );
		}
	}