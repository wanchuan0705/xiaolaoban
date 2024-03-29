export interface ListInt {
  Id: number;
  Name: string;
}
interface SelectDataInt {
  Directory: string;
  page: number; //页码
  count: number; //总条数
  pagesize: number; //默认一页显示几条
}
interface ActiveInt {
  Id: number;
   Name: string;
}
export class InitData {
  selectData: SelectDataInt = {
    Directory: "",
    page: 1,
    count: 0,
    pagesize: 5,
  };
  list: ListInt[] = []; //用来接受用户信息的列表
  isShow = false;
  isAddShow = false;
  isCheckShow = false;
  active: ActiveInt = {
    //选中的对象
    Id: 0,
    Name: "",
  };
}
