请移步到个人博客查看详细教程-------
http://www.cnblogs.com/longm/p/6638694.html

引用基类API：Data.cs、WxPayApi.cs

C#服务端代码：直接获取你需要的二维码链接或者图片
	/// <summary>
    /// 微信二维码支付
    /// </summary>
    private void WeChat()
    {
        Orderid = GetOutTradeNo();//随机数字
        WxPayData data = new WxPayData();
        data.SetValue("body", GoodsName);//商品描述
        data.SetValue("attach", Gamename);//附加数据
        data.SetValue("out_trade_no", Orderid);//随机字符串
        data.SetValue("total_fee", Price);//总金额 (分)正式
        data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
        data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
        data.SetValue("goods_tag", "cyyx");//商品标记
        data.SetValue("trade_type", "NATIVE");//交易类型
        data.SetValue("product_id", "100");//商品ID
        data.SetValue("notify_url", "http://game.aijiexun.com/Pay/WeChatPay/Notify.aspx");//回调地址

        WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
        string returncode = result.GetValue("return_code").ToString();
        string returnmsg = result.GetValue("return_msg").ToString();
        string returndata;//二维码结果，以下分为两种模式1、链接  2、图片。
        if (returncode == "SUCCESS" )
        {
            //returndata = "QRcode.aspx?data=" + result.GetValue("code_url");//获得统一下单接口返回的二维码链接---QRcode.aspx代码在下面
            string codeurl = result.GetValue("code_url").ToString();
            returndata = CreateImg(codeurl, Orderid);//根据二维码链接生成本地图片并展示
            Json = "{\"code\":\"1\",\"data\":\"" + returndata + "\"}";
        }
        else
        {
            returndata = returnmsg;
            Json = "{\"code\":\"0\",\"data\":\"" + returndata + "\"}";
        }
    }
	
	/// <summary>
    /// 生成图片
    /// </summary>
    /// <param name="codeurl">二维码链接</param>
    /// <returns></returns>
    private string CreateImg(string codeurl, string orderid)
    {
        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        qrCodeEncoder.QRCodeVersion = 0;
        qrCodeEncoder.QRCodeScale = 4;

        //将字符串生成二维码图片
        System.Drawing.Image image = qrCodeEncoder.Encode(codeurl,Encoding.Default);
        string filename = orderid + ".jpg";//DateTime.Now.ToString("yyyymmddhhmmssfff") + ".jpg";

        string path1 = Server.MapPath(@"~\upload\QRCode");
        if (!Directory.Exists(path1))
        {
            Directory.CreateDirectory(path1);
        }
        string filepath = Server.MapPath(@"~\upload\QRCode") + "\\" + filename;
        FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
        image.Save(fs, ImageFormat.Jpeg);
        fs.Close();
        image.Dispose();
        string imageUrl = "/upload/QRCode/" + filename;
        return imageUrl;
    }
	
	
	QRcode.aspx代码：
	if (!string.IsNullOrEmpty(Request.QueryString["data"]))
        {
            string str = Request.QueryString["data"];

            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(str, Encoding.Default);

            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            //输出二维码图片
            Response.BinaryWrite(ms.GetBuffer());
            Response.End();
        }
	