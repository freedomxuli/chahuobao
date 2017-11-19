/*此处加载layer.js插件，若要变换位置，请修改layer.js里引用layer.css的路径*/
document.write(" <script src='../../Content/H+/js/plugins/layer/layer.min.js' > <\/script>");
(function() {
       
        var pageJs = {
        /*
         函数：把日期字符串转换成日期对象
         参数：日期字符串
         调用方式：var date=tms.ToDateTime(dateStr);
         返回：日期对象
         */
        ToDateTime: function (dateStr) {
            return dateStr instanceof Date ? dateStr : new Date(dateStr.replace(/-/g, "/"));
        },
        /*
         *  函数：Ajax请求需要的参数
         *  参数：url：请求的数据地址,data:url参数集合，async：是否是异步请求，backFunc:回掉函数，errorFunc错误回掉函数
         *  调用方法 Tms.ajaxParam()
         * */
        ajaxParam: function (url, data, async, backFunc, errorfunc) {
            return {
                url: url,
                data: data,
                async: async,
                backFunc: backFunc,
                errorFunc: errorfunc
            };
        },
        /*
         *  函数：Ajax请求处理函数
         *  参数：obj：obj里的对象请查看ajaxParam函数说明
         *  调用方法 Tms.Ajax(obj)
         * */
        Post: function (obj) {
            $.ajaxSetup({ async: obj.async });
            $.post(
                obj.url,
                obj.data,
                function (backData) {
                    backData = eval("(" + backData + ")");
                    if (!backData || backData.sign == "error") { //|| !backData.Msg
                        if (obj.errorFunc && $.isFunction(obj.errorFunc)) {
                            obj.errorFunc(backData);
                            return;
                        } else {
                            alert(backData.msg);
                        }
                        // console.log("Action返回的json数据格式不正确。错误消息：" + backData.msg + "请求的url为：" + obj.url); //输出错误到控制台
                    }
                    if (obj.backFunc && $.isFunction(obj.backFunc)) {
                        obj.backFunc(backData);
                    }
                }
            ).error(function () {
                alert("网络有问题，请稍后操作"+backdata);
            });
        },

        /*
         *  函数：判断参数param是否包含汉字
         *  参数：param 要判断的字符串
         *  调用方法 Tms.IsChar("测试")
         * */
        IsChar: function (param) {
            if ((/^[\u4e00-\u9fa5]+$/i).test(param)) {
                alert("不能为汉字，请重新输入！");
                return;
            }
        },
        /*
        *  函数：判断name是否全部都是汉字
         * */
        CheckName: function (name) {
            if (!/^[\u4e00-\u9fa5]+$/i.test(name)) {
                Tms.AlertMsg("姓名必须是中文");
                return false;
            } else {
                return true;
            }
        },

        /*
        *  函数：获取分页控件的页数
        *  参数：集合数据长度，显示的条数（默认10条）
        *  调用方法 Tms.PageNum(20,10)
        * */
        PageNum: function (len, count) {
            count = !count === true ? 10 : count;
            var jlen = len % count;
            return jlen === 0 ? (len - jlen) / 10 : (len - jlen) / 10 + 1;
        },
        /**
          * 检验18位身份证号码 6位地区码+8位出生日期码+3位数字顺序码+1位数字效验码
          * 参数：要验证的字符串
          * 返回值：true （验证成功）
          **/
        IsIdCard: function (cid) {
            var arrExp = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2]; //加权因子
            var arrValid = [1, 0, "X", 9, 8, 7, 6, 5, 4, 3, 2]; //校验码
            if (/^\d{17}\d|x$/i.test(cid)) {
                var sum = 0;
                for (var i = 0; i < cid.length - 1; i++) {
                    // 对前17位数字与权值乘积求和
                    sum += parseInt(cid.substr(i, 1), 10) * arrExp[i];
                }
                // 计算模（固定算法）
                var idx = sum % 11;
                // 检验第18为是否与校验码相等
                return arrValid[idx] == cid.substr(17, 1).toUpperCase();
            }
            return false;
        },
        /*
          函数：弹出提示框函数，
          参数：msg：弹出框提示的内容
          调用：Tms.AlertMsg(msg);
        */
        AlertMsg: function (msg) {
            layerAlert(msg); //弹出框提示
        },
        /*
          函数：判断当前页面是否从IFrame进入的，不是的话弹出提示,然后点击确认进入指定页面，
          调用：Tms.IsFrameUrl()
       */
        IsFrameUrl: function () {
            isFrame();
        },
        /*
          函数：弹出提示，然后点击确定重定向当前页面，
          参数：msg：弹出框提示的内容，url：点击弹出框的确定按钮后要重定向的url
          调用：Tms.AlertMsgUrl(msg,url);
        */
        AlertMsgUrl: function (msg, url) {
            alertMsgUrl(msg, url);
        },
        /*
         函数：弹出提示，等待两秒后跳转到登录页面，
         参数：msg：弹出框提示的内容，url：点击弹出框的确定按钮后要重定向的url
         调用：Tms.AlertMsgHref(msg,url);
       */
        AlertMsgHref: function (msg, url) {
            msgHref(msg, url);
        },
        /*
        函数：把包含全角的字符串转换成半角的字符串
        参数：txt 要转成半角字符的字符串
        调用：Tms.FullTranStr(txt);
      */
        FullTranStr: function (txt) {
            var result = "";
            var char = "";
            for (var i = 0; i < txt.length; i++) {
                char = txt.charCodeAt(i); //获得字符编码值
                if (txt.charCodeAt(i) === 12288) //全角空格
                {
                    //result += String.fromCharCode(char - 12256);
                    result += ""; //两种都行

                } else if (char > 65280 && char < 65375) {
                    result += String.fromCharCode(char - 65248); //除空格外，其他全角的字符和半角的字符相差是65248
                } else {
                    result += String.fromCharCode(char); //根据字符编码得到对应的字符
                }
            }
            return result;
        },
        GetUploadParam: function (key, index) {
            return getUploadParam(key, index);
        }, GetDeleteParam: function (key, index) {
            return getPicDelete(key, index);
        },

        Init: function () {
            if ($('.form_datetime').length > 0) {
                $('.form_datetime').datetimepicker({
                    language: 'zh-CN',
                    weekStart: 1,
                    todayBtn: 1,
                    autoclose: 1,
                    todayHighlight: 1,
                    startView: 2,
                    minView: 2,
                    forceParse: 0,
                    initialDate: new Date()
                });
            }

        }
    }
        window.Lbs = pageJs;
})();
Lbs.Init();