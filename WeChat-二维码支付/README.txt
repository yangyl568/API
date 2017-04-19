���Ʋ������˲��Ͳ鿴��ϸ�̳�-------
http://www.cnblogs.com/longm/p/6638694.html

���û���API��Data.cs��WxPayApi.cs

C#����˴��룺ֱ�ӻ�ȡ����Ҫ�Ķ�ά�����ӻ���ͼƬ
	/// <summary>
    /// ΢�Ŷ�ά��֧��
    /// </summary>
    private void WeChat()
    {
        Orderid = GetOutTradeNo();//�������
        WxPayData data = new WxPayData();
        data.SetValue("body", GoodsName);//��Ʒ����
        data.SetValue("attach", Gamename);//��������
        data.SetValue("out_trade_no", Orderid);//����ַ���
        data.SetValue("total_fee", Price);//�ܽ�� (��)��ʽ
        data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//������ʼʱ��
        data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//���׽���ʱ��
        data.SetValue("goods_tag", "cyyx");//��Ʒ���
        data.SetValue("trade_type", "NATIVE");//��������
        data.SetValue("product_id", "100");//��ƷID
        data.SetValue("notify_url", "http://game.aijiexun.com/Pay/WeChatPay/Notify.aspx");//�ص���ַ

        WxPayData result = WxPayApi.UnifiedOrder(data);//����ͳһ�µ��ӿ�
        string returncode = result.GetValue("return_code").ToString();
        string returnmsg = result.GetValue("return_msg").ToString();
        string returndata;//��ά���������·�Ϊ����ģʽ1������  2��ͼƬ��
        if (returncode == "SUCCESS" )
        {
            //returndata = "QRcode.aspx?data=" + result.GetValue("code_url");//���ͳһ�µ��ӿڷ��صĶ�ά������---QRcode.aspx����������
            string codeurl = result.GetValue("code_url").ToString();
            returndata = CreateImg(codeurl, Orderid);//���ݶ�ά���������ɱ���ͼƬ��չʾ
            Json = "{\"code\":\"1\",\"data\":\"" + returndata + "\"}";
        }
        else
        {
            returndata = returnmsg;
            Json = "{\"code\":\"0\",\"data\":\"" + returndata + "\"}";
        }
    }
	
	/// <summary>
    /// ����ͼƬ
    /// </summary>
    /// <param name="codeurl">��ά������</param>
    /// <returns></returns>
    private string CreateImg(string codeurl, string orderid)
    {
        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        qrCodeEncoder.QRCodeVersion = 0;
        qrCodeEncoder.QRCodeScale = 4;

        //���ַ������ɶ�ά��ͼƬ
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
	
	
	QRcode.aspx���룺
	if (!string.IsNullOrEmpty(Request.QueryString["data"]))
        {
            string str = Request.QueryString["data"];

            //��ʼ����ά�����ɹ���
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //���ַ������ɶ�ά��ͼƬ
            Bitmap image = qrCodeEncoder.Encode(str, Encoding.Default);

            //����ΪPNG���ڴ���  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            //�����ά��ͼƬ
            Response.BinaryWrite(ms.GetBuffer());
            Response.End();
        }
	