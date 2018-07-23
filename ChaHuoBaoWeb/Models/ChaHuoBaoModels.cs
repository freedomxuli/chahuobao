using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChaHuoBaoWeb.Models
{
    public class ChaHuoBaoModels : DbContext
    {
        public ChaHuoBaoModels()
            : base("DefaultConnection")
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<QuanXian> QuanXian { get; set; }
        public DbSet<User_Role> User_Role { get; set; }
        public DbSet<Role_QuanXian> Role_QuanXian { get; set; }
        public DbSet<YunDan> YunDan { get; set; }
        public DbSet<YunDanIsArrive> YunDanIsArrive { get; set; }
        public DbSet<YunDanDistance> YunDanDistance { get; set; }
        public DbSet<GpsLocation> GpsLocation { get; set; }
        public DbSet<GpsLocationHis> GpsLocationHis { get; set; }
        public DbSet<GpsLocation2> GpsLocation2 { get; set; }
        public DbSet<GpsDevice> GpsDevice { get; set; }
        public DbSet<GpsDeviceSale> GpsDeviceSale { get; set; }
        public DbSet<GpsDingDan> GpsDingDan { get; set; }
        public DbSet<GpsDingDanSale> GpsDingDanSale { get; set; }
        public DbSet<GpsDingDanGDG> GpsDingDanGDG { get; set; }
        public DbSet<GpsDingDanSaleGDG> GpsDingDanSaleGDG { get; set; }
        public DbSet<ChongZhiGDG> ChongZhiGDG { get; set; }
        public DbSet<GpsDingDanMingXi> GpsDingDanMingXi { get; set; }
        public DbSet<GpsDingDanSaleMingXi> GpsDingDanSaleMingXi { get; set; }
        public DbSet<GpsTuiDan> GpsTuiDan { get; set; }
        public DbSet<GpsTuiDanMingXi> GpsTuiDanMingXi { get; set; }
        public DbSet<ChongZhi> ChongZhi { get; set; }
        public DbSet<InvoiceModel> InvoiceModel { get; set; }
        public DbSet<CaoZuoJiLu> CaoZuoJiLu { get; set; }
        public DbSet<JiaGeCeLve> JiaGeCeLve { get; set; }
        public DbSet<XiTongCanShu> XiTongCanShu { get; set; }
        public DbSet<GpsDeviceTable> GpsDeviceTable { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<ZiYouSearch> ZiYouSearch { get; set; }
    }
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [Display(Name = "用户名称")]
        public string UserName { get; set; }
        [Display(Name = "用户类型")]
        public string UserLeiXing { get; set; }
        [Display(Name = "用户名称说明")]
        public string UserNameDescribe { get; set; }
        [Display(Name = "用户密码")]
        public string UserPassword { get; set; }
        [Display(Name = "用户创建时间")]
        public DateTime UserCreateTime { get; set; }
        [Display(Name = "用户过期时间")]
        public DateTime? UserEndTime { get; set; }
        [Display(Name = "允许登录")]
        public bool UserIsLimit { get; set; }
        [Display(Name = "所属城市")]
        public string UserCity { get; set; }
        [Display(Name = "剩余次数")]
        public int UserRemainder { get; set; }
        [Display(Name = "用户备注")]
        public string UserRemark { get; set; }
        [Display(Name = "微信查询许可")]
        public bool UserWxEnable { get; set; }
        [ForeignKey("UserID")]
        public ICollection<User_Role> User_Roles { set; get; }
    }
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "角色ID")]
        public string RoleID { get; set; }
        [Display(Name = "角色分组")]
        public string RoleGroup { get; set; }
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }
        [Display(Name = "角色备注")]
        public string RoleRemark { get; set; }


        [ForeignKey("RoleID")]
        public ICollection<Role_QuanXian> Role_QuanXians { set; get; }
        [ForeignKey("RoleID")]
        public ICollection<User_Role> User_Roles { set; get; }

    }
    /// <summary>
    /// 权限表
    /// </summary>
    [Table("QuanXian")]
    public class QuanXian
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "权限ID")]
        public string QuanXianID { get; set; }
        [Display(Name = "权限名称")]
        public string QuanXianName { get; set; }
        [Display(Name = "权限备注")]
        public string QuanXianRemark { get; set; }

        [ForeignKey("QuanXianID")]
        public ICollection<Role_QuanXian> Role_QuanXians { set; get; }
    }
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Table("User_Role")]
    public class User_Role
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "角色用户ID")]
        public int U_RID { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [Display(Name = "角色ID")]
        public string RoleID { get; set; }
        [Display(Name = "用户角色备注")]
        public string U_RRemark { get; set; }
        [Column("RoleID"), InverseProperty("User_Roles")]
        public Role Role { get; set; }
        [Column("UserID"), InverseProperty("User_Roles")]
        public User User { get; set; }
    }
    //[Table("User_Role")]
    //public class UsersInRole
    //{
    //    [Key]
    //    [Column(Order = 0)]
    //    public int UserID { get; set; }
    //    [Key]
    //    [Column(Order = 1)]
    //    public int RoleID { get; set; }
    //    [Column("RoleID"), InverseProperty("UsersInRoles")]
    //    public Role Role { get; set; }
    //    [Column("UserID"), InverseProperty("UsersInRoles")]
    //    public UserProfile User { get; set; }
    //}
    /// <summary>
    /// 角色权限表
    /// </summary>
    [Table("Role_QuanXian")]
    public class Role_QuanXian
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "角色权限ID")]
        public int R_QID { get; set; }
        [Display(Name = "角色ID")]
        public string RoleID { get; set; }
        [Display(Name = "权限ID")]
        public string QuanXianID { get; set; }
        [Display(Name = "角色权限备注")]
        public string R_QRemark { get; set; }
        [Column("RoleID"), InverseProperty("Role_QuanXians")]
        public Role Role { get; set; }
        [Column("QuanXianID"), InverseProperty("Role_QuanXians")]
        public QuanXian QuanXian { get; set; }

    }


    /// <summary>
    /// 用户修改页面
    /// </summary>
    public class UserEditViewModel
    {
        public User user { get; set; }
        public IList<checklst> allroles { get; set; }
        public IList<checklst> selectedroles { get; set; }
        public PostedIds postroleids { get; set; }
    }
    /// <summary>
    /// 角色修改页面
    /// </summary>
    public class RoleEditViewModel
    {
        public Role role { get; set; }
        public IList<checklst> allquanxians { get; set; }
        public IList<checklst> selectedquanxians { get; set; }
        public PostedIds postquanxianids { get; set; }
    }
    public class checklst
    {
        public string id { get; set; }
        public string name { get; set; }
        //public object Tags { get; set; }
        public bool isselected { get; set; }
    }
    public class PostedIds
    {
        public string[] Ids { get; set; }
    }




    /// <summary>
    /// 运单表
    /// </summary>
    [Table("YunDan")]
    public class YunDan
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "运单系统编号")]
        public string YunDanDenno { get; set; }
        [Display(Name = "运单手工编号")]
        public string UserDenno { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User userModelss { get; set; }
        [Display(Name = "起始站")]
        public string QiShiZhan { get; set; }
        [Display(Name = "到达站")]
        public string DaoDaZhan { get; set; }
        [Display(Name = "所属公司")]
        public string SuoShuGongSi { get; set; }
        [Display(Name = "绑定时间")]
        public DateTime BangDingTime { get; set; }
        [Display(Name = "解绑时间")]
        public DateTime? JieBangTime { get; set; }
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "设备vid")]
        public string GpsDevicevid { get; set; }
        [Display(Name = "设备vKey")]
        public string GpsDevicevKey { get; set; }
        [Display(Name = "最新地理纬度")]
        public string Gps_lastlat { get; set; }
        [Display(Name = "最新地理经度")]
        public string Gps_lastlng { get; set; }
        [Display(Name = "最新定位时间")]
        public DateTime? Gps_lasttime { get; set; }
        [Display(Name = "最新地理位置")]
        public string Gps_lastinfo { get; set; }
        [Display(Name = "是否绑定")]
        public bool IsBangding { get; set; }
        [Display(Name = "运单备注")]
        public string YunDanRemark { get; set; }

        [Display(Name = "起始站纬度")]
        public string QiShiZhan_lat { get; set; }
        [Display(Name = "起始站经度")]
        public string QiShiZhan_lng { get; set; }
        [Display(Name = "到达站纬度")]
        public string DaoDaZhan_lat { get; set; }
        [Display(Name = "到达站经度")]
        public string DaoDaZhan_lng { get; set; }
        [Display(Name = "剩余路程")]
        public string Gps_distance { get; set; }
        [Display(Name = "剩余时间")]
        public string Gps_duration { get; set; }
        [Display(Name = "销售员")]
        public string SalePerson { get; set; }
        [Display(Name = "收货单位")]
        public string Purchaser { get; set; }
        [Display(Name = "收货人")]
        public string PurchaserPerson { get; set; }
        [Display(Name = "收货人联系电话")]
        public string PurchaserTel { get; set; }
        [Display(Name = "承运公司")]
        public string CarrierCompany { get; set; }
        [Display(Name = "负责人")]
        public string CarrierPerson { get; set; }
        [Display(Name = "负责人联系电话")]
        public string CarrierTel { get; set; }
        [Display(Name = "到达地详细地址")]
        public string DaoDaAddress { get; set; }
        [Display(Name = "起始地详细地址")]
        public string QiShiAddress { get; set; }
        [Display(Name = "出发提醒")]
        public int? IsChuFaMessage { get; set; }
        [Display(Name = "到达提醒")]
        public int? IsDaoDaMessage { get; set; }
        [Display(Name = "起始地区县")]
        public string QiShiZhan_QX { get; set; }
        [Display(Name = "到达地区县")]
        public string DaoDaZhan_QX { get; set; }
        [Display(Name = "预计小时数")]
        public decimal? Expect_Hour { get; set; }
    }

    [Table("YunDanIsArrive")]
    public class YunDanIsArrive
    { 
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "主键ID")]
        public string ID { get; set; }
        [Display(Name = "运单系统编号")]
        public string YunDanDenno { get; set; }
        [Display(Name = "最新地理纬度")]
        public string Gps_lastlat { get; set; }
        [Display(Name = "最新地理经度")]
        public string Gps_lastlng { get; set; }
        [Display(Name = "新增时间")]
        public DateTime? Addtime { get; set; }
        [Display(Name = "最新地理位置")]
        public string Gps_lastinfo { get; set; }
        [Display(Name = "所属公司")]
        public string Company { get; set; }
    }

    /// <summary>
    /// GPS剩余路程表
    /// </summary>
    [Table("YunDanDistance")]
    public class YunDanDistance
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "主键ID")]
        public Int32 ID { get; set; }
        [Display(Name = "运单系统编号")]
        public string YunDanDenno { get; set; }
        [Display(Name = "运单手工编号")]
        public string UserDenno { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "最新地理纬度")]
        public string Gps_lastlat { get; set; }
        [Display(Name = "最新地理经度")]
        public string Gps_lastlng { get; set; }
        [Display(Name = "最新定位时间")]
        public DateTime? Gps_lasttime { get; set; }
        [Display(Name = "剩余路程")]
        public string Gps_distance { get; set; }
        [Display(Name = "剩余时间")]
        public string Gps_duration { get; set; }
    }

    /// <summary>
    /// GPS定位表
    /// </summary>
    [Table("GpsLocation")]
    public class GpsLocation
    {
        [Key]
        [Display(Name = "Gps定位ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GpsLocationID { get; set; }
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "地理纬度")]
        public string Gps_lat { get; set; }
        [Display(Name = "地理经度")]
        public string Gps_lng { get; set; }
        [Display(Name = "定位时间")]
        public DateTime Gps_time { get; set; }
        [Display(Name = "地理位置")]
        public string Gps_info { get; set; }
        [Display(Name = "备注")]
        public string GpsRemark { get; set; }
    }

    /// <summary>
    /// GPS定位表
    /// </summary>
    [Table("GpsLocationHis")]
    public class GpsLocationHis
    {
        [Key]
        [Display(Name = "Gps定位ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GpsLocationID { get; set; }
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "地理纬度")]
        public string Gps_lat { get; set; }
        [Display(Name = "地理经度")]
        public string Gps_lng { get; set; }
        [Display(Name = "定位时间")]
        public DateTime Gps_time { get; set; }
        [Display(Name = "地理位置")]
        public string Gps_info { get; set; }
        [Display(Name = "备注")]
        public string GpsRemark { get; set; }
    }

    /// <summary>
    /// GPS定位表2
    /// </summary>
    [Table("GpsLocation2")]
    public class GpsLocation2
    {
        [Key]
        [Display(Name = "Gps定位ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GpsLocationID { get; set; }
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "地理纬度")]
        public string Gps_lat { get; set; }
        [Display(Name = "地理经度")]
        public string Gps_lng { get; set; }
        [Display(Name = "定位时间")]
        public DateTime Gps_time { get; set; }
        [Display(Name = "地理位置")]
        public string Gps_info { get; set; }
        [Display(Name = "备注")]
        public string GpsRemark { get; set; }
    }

    /// <summary>
    /// 自由查单操作表
    /// </summary>
    [Table("ZiYouSearch")]
    public class ZiYouSearch
    { 
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [Display(Name = "所属公司")]
        public string SuoShuGongSi { get; set; }
        [Display(Name = "用户订单")]
        public string UserDenno { get; set; }
    }

    /// <summary>
    /// GPS设备表
    /// </summary>
    [Table("GpsDevice")]
    public class GpsDevice
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User usermodel { get; set; }
        [Display(Name = "备注")]
        public string GpsDeviceRemark { get; set; }
    }

    /// <summary>
    /// GPS设备表
    /// </summary>
    [Table("GpsDeviceSale")]
    public class GpsDeviceSale
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User usermodel { get; set; }
        [Display(Name = "备注")]
        public string GpsDeviceRemark { get; set; }
    }

    /// <summary>
    /// 设备订单表
    /// </summary>
    [Table("GpsDingDan")]
    public class GpsDingDan
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "设备订单单号")]
        public string GpsDingDanDenno { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User userModel { get; set; }
        [Display(Name = "设备订单生成状态")]
        public bool GpsDingDanIsEnd { get; set; }
        [Display(Name = "设备订单数量")]
        public int GpsDingDanShuLiang { get; set; }
        [Display(Name = "设备订单金额")]
        public decimal GpsDingDanJinE { get; set; }
        [Display(Name = "设备订单支付类型")]
        public string GpsDingDanZhiFuLeiXing { get; set; }
        [Display(Name = "设备订单支付状态")]
        public bool GpsDingDanZhiFuZhuangTai { get; set; }
        [Display(Name = "设备订单时间")]
        public DateTime GpsDingDanTime { get; set; }
        [Display(Name = "设备订单支付时间")]
        public DateTime? GpsDingDanZhiFuShiJian { get; set; }
        [Display(Name = "设备订单备注")]
        public string GpsDingDanRemark { get; set; }
        [Display(Name = "设备订单支付单号")]
        public string OrderDenno { get; set; }
        [Display(Name = "公对公审核状态")]
        public bool GpsDingDanSH { get; set; }
        //[ForeignKey("GpsDingDanDenno")]
        //public ICollection<GpsDingDanMingXi> GpsDingDanMingXiList { set; get; }

    }
    /// <summary>
    /// 设备销售订单表
    /// </summary>
    [Table("GpsDingDanSale")]
    public class GpsDingDanSale
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "设备订单单号")]
        public string GpsDingDanDenno { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User userModel { get; set; }
        [Display(Name = "设备订单生成状态")]
        public bool GpsDingDanIsEnd { get; set; }
        [Display(Name = "设备订单数量")]
        public int GpsDingDanShuLiang { get; set; }
        [Display(Name = "设备订单金额")]
        public decimal GpsDingDanJinE { get; set; }
        [Display(Name = "设备订单支付类型")]
        public string GpsDingDanZhiFuLeiXing { get; set; }
        [Display(Name = "设备订单支付状态")]
        public bool GpsDingDanZhiFuZhuangTai { get; set; }
        [Display(Name = "设备订单时间")]
        public DateTime GpsDingDanTime { get; set; }
        [Display(Name = "设备订单支付时间")]
        public DateTime? GpsDingDanZhiFuShiJian { get; set; }
        [Display(Name = "设备订单备注")]
        public string GpsDingDanRemark { get; set; }
        [Display(Name = "设备订单支付单号")]
        public string OrderDenno { get; set; }
        [Display(Name = "公对公审核状态")]
        public bool GpsDingDanSH { get; set; }
        //[ForeignKey("GpsDingDanDenno")]
        //public ICollection<GpsDingDanMingXi> GpsDingDanMingXiList { set; get; }

    }
    /// <summary>
    /// 设备订单明细表
    /// </summary>
    [Table("GpsDingDanMingXi")]
    public class GpsDingDanMingXi
    {
        [Key]
        [Display(Name = "设备订单明细ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GpsDingDanMingXiID { get; set; }
        [Display(Name = "设备订单单号")]
        public string GpsDingDanDenno { get; set; }
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "设备订单明细扫描时间")]
        public DateTime GpsDingDanMingXiTime { get; set; }
        [Display(Name = "设备订单明细备注")]
        public string GpsDingDanMingXiRemark { get; set; }
        [ForeignKey("GpsDingDanDenno")]
        public GpsDingDan GpsDingDanModel { get; set; }
    }

    /// <summary>
    /// 设备销售订单明细表
    /// </summary>
    [Table("GpsDingDanSaleMingXi")]
    public class GpsDingDanSaleMingXi
    {
        [Key]
        [Display(Name = "设备订单明细ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GpsDingDanMingXiID { get; set; }
        [Display(Name = "设备订单单号")]
        public string GpsDingDanDenno { get; set; }
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "设备订单明细扫描时间")]
        public DateTime GpsDingDanMingXiTime { get; set; }
        [Display(Name = "设备订单明细备注")]
        public string GpsDingDanMingXiRemark { get; set; }
        [ForeignKey("GpsDingDanDenno")]
        public GpsDingDanSale GpsDingDanSaleModel { get; set; }
    }

    [Table("GpsDingDanGDG")]
    public class GpsDingDanGDG
    { 
        [Key]
        [Display(Name = "公对公支付主键")]
        public string GDGZhiFu { get; set; }
        [Display(Name = "设备订单支付单号")]
        public string OrderDenno { get; set; }
        [Display(Name = "对公转账公司名称")]
        public string DGZZCompany { get; set; }
        [Display(Name = "对公账户")]
        public string DGZH { get; set; }
        [Display(Name = "打款凭证号")]
        public string DKPZH { get; set; }
        [Display(Name = "生成时间")]
        public DateTime AddTime { get; set; }
        [ForeignKey("OrderDenno")]
        public GpsDingDan GpsDingDanModel { get; set; }
    }

    [Table("GpsDingDanSaleGDG")]
    public class GpsDingDanSaleGDG
    {
        [Key]
        [Display(Name = "公对公支付主键")]
        public string GDGZhiFu { get; set; }
        [Display(Name = "设备订单支付单号")]
        public string OrderDenno { get; set; }
        [Display(Name = "对公转账公司名称")]
        public string DGZZCompany { get; set; }
        [Display(Name = "对公账户")]
        public string DGZH { get; set; }
        [Display(Name = "打款凭证号")]
        public string DKPZH { get; set; }
        [Display(Name = "生成时间")]
        public DateTime AddTime { get; set; }
        [ForeignKey("OrderDenno")]
        public GpsDingDanSale GpsDingDanModel { get; set; }
    }

    /// <summary>
    /// 设备退单表
    /// </summary>
    [Table("GpsTuiDan")]
    public class GpsTuiDan
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "设备退单单号")]
        public string GpsTuiDanDenno { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User userModels { get; set; }
        [Display(Name = "设备退单生成状态")]
        public bool GpsTuiDanIsEnd { get; set; }
        [Display(Name = "设备退单数量")]
        public int GpsTuiDanShuLiang { get; set; }
        [Display(Name = "设备退单金额")]
        public decimal GpsTuiDanJinE { get; set; }
        [Display(Name = "设备退单账号")]
        public string GpsTuiDanZhangHao { get; set; }
        [Display(Name = "设备退单时间")]
        public DateTime GpsTuiDanTime { get; set; }
        [Display(Name = "设备退单审核状态")]
        public bool GpsTuiDanShenHeZhuangTai { get; set; }
        [Display(Name = "设备退单审核时间")]
        public DateTime? GpsTuiDanShenHeShiJian { get; set; }
        [Display(Name = "设备退单退还状态")]
        public bool GpsTuiDanTuiHuanZhuangTai { get; set; }
        [Display(Name = "设备退单退还时间")]
        public DateTime? GpsTuiDanTuiHuanShiJian { get; set; }
        [Display(Name = "设备退单备注")]
        public string GpsTuiDanRemark { get; set; }
        [Display(Name = "设备退单支付单号")]
        public string OrderDenno { get; set; }
        [Display(Name = "设备退单申请状态")]
        public bool GpsTuiDanIsShenQing { get; set; }
        [Display(Name = "设备退单申请时间")]
        public DateTime? GpsTuiDanShenQingTime { get; set; }

    }
    /// <summary>
    /// 设备退单明细表
    /// </summary>
    [Table("GpsTuiDanMingXi")]
    public class GpsTuiDanMingXi
    {
        [Key]
        [Display(Name = "设备退单明细ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GpsTuiDanMingXiID { get; set; }
        [Display(Name = "设备退单单号")]
        public string GpsTuiDanDenno { get; set; }
        [Display(Name = "设备ID")]
        public string GpsDeviceID { get; set; }
        [Display(Name = "设备订单明细扫描时间")]
        public DateTime GpsTuiDanMingXiTime { get; set; }
        [Display(Name = "设备订单明细备注")]
        public string GpsTuiDanMingXiRemark { get; set; }
        [ForeignKey("GpsTuiDanDenno")]
        public GpsTuiDan GpsTuiDanModel { get; set; }
    }
    /// <summary>
    /// 充值表
    /// </summary>
    [Table("ChongZhi")]
    public class ChongZhi
    {
        [Key]
        [Display(Name = "充值ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ChongZhiID { get; set; }
        public string OrderDenno { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User uesrmodell { get; set; }
        [Display(Name = "充值金额")]
        public decimal ChongZhiJinE { get; set; }
        [Display(Name = "充值次数")]
        public int ChongZhiCiShu { get; set; }
        [Display(Name = "充值说明")]
        public string ChongZhiDescribe { get; set; }
        [Display(Name = "充值时间")]
        public DateTime ChongZhiTime { get; set; }
        [Display(Name = "支付时间")]
        public DateTime? ZhiFuTime { get; set; }
        [Display(Name = "支付状态")]
        public bool ZhiFuZhuangTai { get; set; }
        [Display(Name = "充值备注")]
        public string ChongZhiRemark { get; set; }
        [Display(Name = "充值审核")]
        public bool ChongZhiSH { get; set; }
        [Display(Name = "审核是否通过")]
        public int? IsShPass { get; set; }
    }

    [Table("InvoiceModel")]
    public class InvoiceModel
    { 
        [Key]
        [Display(Name = "发票主键id")]
        public string InvoiceId { get; set; }
        [Display(Name = "发票抬头")]
        public string InvoiceTitle { get; set; }
        [Display(Name = "组织机构代码")]
        public string InvoiceZZJGDM { get; set; }
        [Display(Name = "联系人")]
        public string InvoicePerson { get; set; }
        [Display(Name = "联系电话")]
        public string InvoiceMobile { get; set; }
        [Display(Name = "寄送地址")]
        public string InvoiceAddress { get; set; }
        [Display(Name = "用户id")]
        public string UserId { get; set; }
        [Display(Name = "申领时间")]
        public DateTime? AddTime { get; set; }
        [Display(Name = "寄送状态")]
        public bool IsOut { get; set; }
        [Display(Name = "发票金额")]
        public decimal InvoiceJe { get; set; }
        [ForeignKey("UserId")]
        public User userModelss { get; set; }
    }

    [Table("ChongZhiGDG")]
    public class ChongZhiGDG
    {
        [Key]
        [Display(Name = "公对公支付主键")]
        public string GDGChongZhi { get; set; }
        [Display(Name = "设备订单支付单号")]
        public string OrderDenno { get; set; }
        [Display(Name = "对公转账公司名称")]
        public string DGZZCompany { get; set; }
        [Display(Name = "对公账户")]
        public string DGZH { get; set; }
        [Display(Name = "打款凭证号")]
        public string DKPZH { get; set; }
        [Display(Name = "生成时间")]
        public DateTime AddTime { get; set; }
        //[ForeignKey("OrderDenno")]
        //public ChongZhi ChongZhiModel { get; set; }
    }

    /// <summary>
    /// 操作记录表
    /// </summary>
    [Table("CaoZuoJiLu")]
    public class CaoZuoJiLu
    {
        [Key]
        [Display(Name = "操作记录ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CaoZuoJiLuID { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User userModelt { get; set; }
        [Display(Name = "操作类型")]
        public string CaoZuoLeiXing { get; set; }
        [Display(Name = "操作内容")]
        public string CaoZuoNeiRong { get; set; }
        [Display(Name = "操作时间")]
        public DateTime CaoZuoTime { get; set; }
        [Display(Name = "操作备注")]
        public string CaoZuoRemark { get; set; }
    }
    /// <summary>
    /// 价格策略表
    /// </summary>
    [Table("JiaGeCeLve")]
    public class JiaGeCeLve
    {
        [Key]
        [Display(Name = "价格策略ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int JiaGeCeLveID { get; set; }
        [Display(Name = "价格策略名称")]
        public string JiaGeCeLveName { get; set; }

        [Display(Name = "价格策略类型")]
        public string JiaGeCeLveLeiXing { get; set; }

        [Display(Name = "价格策略金额")]
        public decimal JiaGeCeLveJinE { get; set; }
        [Display(Name = "价格策略次数")]
        public int JiaGeCeLveCiShu { get; set; }
        [Display(Name = "价格策略备注")]
        public string JiaGeCeLveRemark { get; set; }
    }

    /// <summary>
    /// 系统参数配置
    /// </summary>
    [Table("XiTongCanShu")]
    public class XiTongCanShu
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "值")]
        public string Value { get; set; }
    }
    /// <summary>
    /// 搜索历史
    /// </summary>
    [Table("SearchHistory")]
    public class SearchHistory
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
        [Display(Name = "类型")]
        public string Type { get; set; }
        [Display(Name = "值")]
        public string Value { get; set; }
    }

    /// <summary>
    /// 系统参数配置
    /// </summary>
    [Table("GpsDeviceTable")]
    public class GpsDeviceTable
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Display(Name = "设备前缀号")]
        public string DeviceCode { get; set; }
        [Display(Name = "对应数据表")]
        public string TableName { get; set; }
        [Display(Name = "设置同步的分钟数")]
        public int DeviceTime { get; set; }
    }
}