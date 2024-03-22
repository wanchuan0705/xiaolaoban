export interface ListInt {
  Id: number;
  Account: string;
  Password: string;
  Phone: string;
  Name: string;
  Age: string;
  Sex: string;
  Role: string;
}
interface SelectDataInt {
  Name: string;
}
interface RoleListInt {
  authority: number[];
  roleId: number;
  roleName: string;
}
interface ActiveInt {
  Id: number;
  Account: string;
  Password: string;
  Phone: string;
  Name: string;
  Age: string;
  Sex: string;
  Role: string;
}
export class InitData {
  selectData: SelectDataInt = {
    Name: "",
  };
  judgeAdd: number=0;
  list: ListInt[] = []; //用来接受用户信息的列表
  roleList: RoleListInt[] = []; //用来接受角色信息的列表
  isShow = false;
  isAddShow = false;
  active: ActiveInt = {
    //选中的对象
    Id: 0,
    Account: "",
    Password: "",
    Phone: "",
    Name: "",
    Age: "",
    Sex: "",
    Role: "",
  };
}
