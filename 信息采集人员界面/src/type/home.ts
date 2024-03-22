export interface ListInt {
  id: number;
  name: string;
  password: string;
  phone: string;
}
interface ActiveInt {
   id: number;
  name: string;
  password: string;
  phone: string;
}
export class InitData {
    list: ListInt[] = []; //用来接受用户信息的列表
    isAddShow= false;
  active2: ActiveInt = {
    //选中的对象
    id: 0,
    name: "",
    password: "",
    phone: "",
  };
}
