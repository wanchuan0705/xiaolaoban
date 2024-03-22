export interface ListInt {
  CaseNo: string;
Id: number;
Address: string;
Types: string;
Details: string;
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
M_Content: string;
Content: string;
PolicerName1: string;
ViolatorsPhone: string;
OrderTake: string;
LegalArticles: string;

}
interface SelectDataInt {
  status: string;
  page: number; //页码
  count: number; //总条数
  pagesize: number; //默认一页显示几条
}
interface ActiveInt {
   CaseNo: string;
Id: number;
Address: string;
Types: string;
Details: string;
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
M_Content: string;
Content: string;
PolicerName1: string;
ViolatorsPhone: string;
OrderTake: string;
LegalArticles: string;

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
   CaseNo:"",
    Id: 0,
  Address: "",
  Types: "",
  Details: "",
  Platenumber: "",
  Date: new Date(),
  Phone: "",
  Image: "",
  PolicerName: "",
  State: "",
  ViolatorsName: "",
  ApplicationTime: "",
  Description: "",
  Model: "",
  Judgment: "",
  M_Content: "",
  Content: "",
  PolicerName1: "",
  ViolatorsPhone: "",
  OrderTake: "",
  LegalArticles: ""
  };
}
