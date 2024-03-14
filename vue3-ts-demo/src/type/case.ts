export interface ListInt {
  Id: number;
  Address: string;
  Details: string;
  Types: string;
  Platenumber: string;
  Date: Date;
  Phone: string;
  Image: string;
  PolicerName: string;
  State: string;
  ViolatorsName: string;
  ApplicationTime: string;
  Description: string;
  Model: string;
  Judgment: string;
  M_Conten: string;
  PolicerName1: string;
  ViolatorsPhone: string;
}
interface SelectDataInt {
  status: string;
  page: number; //页码
  count: number; //总条数
  pagesize: number; //默认一页显示几条
}
interface ActiveInt {
  Id: number;
  Address: string;
  Details: string;
  Types: string;
  Platenumber: string;
  Date: Date;
  Phone: string;
  Image: string;
  PolicerName: string;
  State: string;
  ViolatorsName: string;
  ApplicationTime: string;
  Description: string;
  Model: string;
  Judgment: string;
  M_Conten: string;
  PolicerName1: string;
  ViolatorsPhone: string;
}
export class InitData {
  selectData: SelectDataInt = {
    status: "",
    page: 1,
    count: 0,
    pagesize: 5,
  };
  list: ListInt[] = []; //用来接受用户信息的列表
  isShow = false;
  isAddShow = false;
  active: ActiveInt = {
    //选中的对象
    Id: 0,
    Address: "",
    Details: "",
    Types: "",
    Platenumber: "",
    Date: new Date(), // 默认为当前时间
    Phone: "",
    Image: "",
    PolicerName: "",
    State: "",
    ViolatorsName: "",
    ApplicationTime: "",
    Description: "",
    Model: "",
    Judgment: "",
    M_Conten: "",
    PolicerName1: "",
    ViolatorsPhone: "",
  };
}
