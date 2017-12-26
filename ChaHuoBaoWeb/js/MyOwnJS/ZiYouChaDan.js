function ziyouchadan_load(SuoShuGongSi, UserDenno) {
    setCookie('SuoShuGongSi', SuoShuGongSi, 365);
    setCookie('UserDenno', UserDenno, 365);
	var ajax_sign;
	var ajax_msg;
	var UserName = "";
	//	mui.ajax("http://192.168.1.109:7287/WebService/APP_ZiYouChaDan.ashx", {
	mui.ajax("http://chb.yk56.net/WebService/APP_ZiYouChaDan.ashx", {
		dataType: "json",
		type: "post",
		data: {
			"UserName": UserName,
			"SuoShuGongSi": SuoShuGongSi,
			"UserDenno": UserDenno
		},
		success: function(data, status, xhr) {
			if(data.sign == '0') {
				mui.alert(data.msg);
			}
			if(data.sign == '2') {
				document.getElementById("noyundan").style.visibility = "visible";
				document.getElementById("slider").style.visibility = "hidden";
			}
			if(data.sign == '1') {
				document.getElementById("noyundan").style.visibility = "hidden";
				document.getElementById("slider").style.visibility = "visible";
				document.getElementById("wodeyundan_list").innerHTML = "";
				var yundanlist = null;
				yundanlist = data.yundanlist;
				var table = document.body.querySelector('#wodeyundan_list');
				var cells = document.body.querySelectorAll('.mui-table-view-cell');
				var cell_length = cells.length - 1;
				for(var i = 0; i < yundanlist.length; i++) {
					cell_length = cell_length + 1
					var time = yundanlist[i].BangDingTime.substring(5, 10);
					var li = document.createElement('li');
					li.className = 'mui-table-view-cell';
					li.id = cell_length;
					li.name = yundanlist[i].UserID;
					li.tap = yundanlist[i].YunDanDenno;

					var dm = "";
					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">日期:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += time;
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">运单号:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].UserDenno;
					dm += '</label></div>';

					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">起始站:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					if (yundanlist[i].QiShiZhan_QX != null && yundanlist[i].QiShiZhan_QX != "")
					    dm += yundanlist[i].QiShiZhan + " " + yundanlist[i].QiShiZhan_QX;
					else
					    dm += yundanlist[i].QiShiZhan
					dm += '</label></div>';

					if (yundanlist[i].QiShiAddress != null && yundanlist[i].QiShiAddress != "") {
					    dm += '<div class="mui-input-row ">';
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">详细地址:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].QiShiAddress
					    dm += '</label></div>';
					}

					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">到达站:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					if (yundanlist[i].DaoDaZhan_QX != null && yundanlist[i].DaoDaZhan_QX != "")
					    dm += yundanlist[i].DaoDaZhan + " " + yundanlist[i].DaoDaZhan_QX;
					else
					    dm += yundanlist[i].DaoDaZhan;
					dm += '</label></div>';

					if (yundanlist[i].DaoDaAddress != null && yundanlist[i].DaoDaAddress != "") {
					    dm += '<div class="mui-input-row ">';
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">详细地址:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].DaoDaAddress
					    dm += '</label></div>';
					}

					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">公司名称:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].SuoShuGongSi;
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">设备ID:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].GpsDeviceID;
					dm += '</label></div>';

				    //					dm += '<div class="mui-input-row ">';
				    //					dm += '</label><label style="width: 35%; padding-left: 0px; padding-right: 0px">公司名称:</label><label style="width: 65%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
				    //					dm += yundanlist[i].SuoShuGongSi;
				    //					dm += '</label></div>';
					if (yundanlist[i].SalePerson != null && yundanlist[i].SalePerson != "") {
					    dm += '<div class="mui-input-row ">';
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">销售员:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].SalePerson
					    dm += '</label></div>';
					}

					if (yundanlist[i].Purchaser != null && yundanlist[i].Purchaser != "") {
					    dm += '<div class="mui-input-row ">';
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">收货地址:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].Purchaser
					    dm += '</label></div>';
					}

					dm += '<div class="mui-input-row ">';
					if (yundanlist[i].PurchaserPerson != null && yundanlist[i].PurchaserPerson != "") {
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">收货人:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].PurchaserPerson;
					}
					if (yundanlist[i].PurchaserTel != null && yundanlist[i].PurchaserTel != "") {
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">联系方式:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].PurchaserTel;
					}
					dm += '</label></div>';

					if (yundanlist[i].CarrierCompany != null && yundanlist[i].CarrierCompany != "") {
					    dm += '<div class="mui-input-row ">';
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">承运公司:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].CarrierCompany
					    dm += '</label></div>';
					}

					dm += '<div class="mui-input-row ">';
					if (yundanlist[i].CarrierPerson != null && yundanlist[i].CarrierPerson != "") {
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">负责人:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].CarrierPerson;
					}
					if (yundanlist[i].CarrierTel != null && yundanlist[i].CarrierTel != "") {
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">联系方式:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    dm += yundanlist[i].CarrierTel;
					}
					dm += '</label></div>';

					if (yundanlist[i].Gps_distance != "" && yundanlist[i].Gps_distance != null)
					{
					    dm += '<div class="mui-input-row ">';
					    dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">剩余路程:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    if (yundanlist[i].Gps_distance) {
					        dm += yundanlist[i].Gps_distance;
					    }
					    dm += ' 公里</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">剩余时间:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					    if (yundanlist[i].Gps_duration) {
					        var hour = Math.floor(yundanlist[i].Gps_duration / 60);
					        var min = Math.ceil(yundanlist[i].Gps_duration % 60);

					        if (hour) {
					            dm += hour + '小时';
					        }
					        dm += min;
					    }

					    dm += ' 分钟</label></div>';
					}

					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">运单备注:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].YunDanRemark;
					dm += '</label></div>';

					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">当前位置:</label><label style="width: 75%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					if (yundanlist[i].Gps_lasttime == null) {
					    dm += yundanlist[i].Gps_lastinfo;
					} else {
					    dm += yundanlist[i].Gps_lastinfo + "(" + yundanlist[i].Gps_lasttime + ")";
					}
					dm += '</label></div>';

					dm += '<p class="mui-btn-link mui-pull-right ">查看轨迹</p></div>';
					li.innerHTML = dm;
					table.appendChild(li);
				}
				mui('.mui-table-view').on('tap', 'li', function(e) {
					window.location.href = 'chadan_yundanguiji.html?UserID=' + this.name + '&YunDanDenno=' + this.tap + '&type=ziyouchadan'
				});
			}
		}
	});
}

function setCookie(c_name, value, expiredays) {
    var exdate = new Date()
    exdate.setDate(exdate.getDate() + expiredays)
    document.cookie = c_name + "=" + escape(value) +
    ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString())
}

//取回cookie
function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}