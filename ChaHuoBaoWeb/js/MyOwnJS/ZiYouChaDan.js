function ziyouchadan_load(SuoShuGongSi, UserDenno) {
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
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">起始站:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].QiShiZhan;
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">到达站:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].DaoDaZhan;
					dm += '</label></div>';

					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">公司名称:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].SuoShuGongSi;
					dm += '</label><label style="width: 25%; padding-left: 0px; padding-right: 0px">设备ID:</label><label style="width: 25%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].GpsDeviceID;
					dm += '</label></div>';


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

					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 35%; padding-left: 0px; padding-right: 0px">运单备注:</label><label style="width: 65%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					dm += yundanlist[i].YunDanRemark;
					dm += '</label></div>';

					dm += '<div class="mui-input-row ">';
					dm += '</label><label style="width: 35%; padding-left: 0px; padding-right: 0px">当前位置:</label><label style="width: 65%; padding-left: 0px; padding-right: 0px;margin-left: -15px;">';
					if(yundanlist[i].Gps_lasttime == null) {
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