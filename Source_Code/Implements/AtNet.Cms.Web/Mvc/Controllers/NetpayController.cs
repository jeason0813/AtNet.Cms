﻿//
// 文件名称: CmsController.cs
// 作者：  newmin
// 创建时间：2012-10-01
// 修改说明：
//  2012-11-12  06:55   newmin  [!]:修改自定义页面的链接
//  2013-01-05  09:02   newmin  [!]:修正特殊文档不能查找到第一个，不立即返回404的错误
//  2013-02-01  10:00   newmin  [+]: 表格提交
//  2013-03-06  11:17   newmin  [+]: 评论模块
//
namespace Spc.Web.Mvc
{
    using System;
    using System.Web.Mvc;
    using Spc;
    using System.Web;
    using Ops.Plugin.NetPay;
    using System.Collections.Specialized;
    using System.Collections.Generic;


    public class CmsNetpayHandle : PaymentHandler<string>
    {
    	
    	
		public override void OnOrderPaidSuccess(string t)
		{
			throw new NotImplementedException();
		}
    	
		public override void OnOrderPaidFail(string t)
		{
			throw new NotImplementedException();
		}
    }

    public class CmsNetpayHandler : System.Web.Routing.IRouteHandler
    {
        public IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
        {
            return new NetpayController();
        }


        /// <summary>
        /// 
        /// </summary>
        public class NetpayController : Spc.Web.Mvc.ControllerBase, IHttpHandler
        {
            private CmsNetpayHandle netpay;
            NameValueCollection form;


            internal string Submit()
            {
                PayMethods pm = (PayMethods)int.Parse(form["pmid"]);
                PayApiType pt = (PayApiType)int.Parse(form["ptid"]);

                string path = global::System.Web.HttpContext.Current.Request.Path;
                return netpay.SubmitRequest(
                    pm,
                    pt,
                    "2088302723942172",
                    "6u01s4nvgwx99iwrl58k3xfzb83wydti",
                    "450441696@qq.com", BankSign.Default,
                    form["order_no"],
                    float.Parse(form["order_fee"]),
                     float.Parse(form["order_exp_fee"]),
                     "支付订单",
                     form["show_url"],
                     null,
                     form["receive_name"],
                     form["receive_address"],
                     form["receive_zip"],
                     form["receive_phone"],
                     form["receive_mobile"],
                     String.Format("{0}?return_{1}_{2}", path, (int)pm, (int)pt),
                     String.Format("{0}?notify_{1}_{2}", path, (int)pm, (int)pt)
                     );
            }


            public void ProcessRequest(HttpContext context)
            {
                netpay = new CmsNetpayHandle();
                form = context.Request.Form;
                string action = context.Request["action"];
                if (action == "submit")
                {
                    context.Response.Write(Submit());
                }
                else if (action == "test")
                {
                    SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
                    dict.Add("action", "submit");
                    dict.Add("order_no", String.Empty.RandomLetters(10));
                    dict.Add("pmid", "1");
                    dict.Add("ptid", "3");
                    dict.Add("order_fee", "0.01");
                    dict.Add("order_exp_fee", "0");
                    dict.Add("show_url", "");
                    dict.Add("receive_name", "");
                    dict.Add("receive_address", "");
                    dict.Add("receive_zip", "");
                    dict.Add("receive_phone", "");
                    dict.Add("receive_mobile", "");

                    context.Response.Write(
                        PayUtil.GetGatewaySubmit(context.Request.Path, dict)
                        );

                }
            }

            public bool IsReusable
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
