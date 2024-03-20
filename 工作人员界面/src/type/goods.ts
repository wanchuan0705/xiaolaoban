export interface ListInt{
    userId:number,
    id:number,
    title:string,
    introduce:string
}
interface selectDataInt{
    title:string,
    introduce:string,
    page:number,//页码
    count:number,//总条数
    pagesize:number,//默认一页显示几条
}
export class InitData{
    selectData:selectDataInt={
        title:"",
        introduce:"",
        page:1,
        count:0,
        pagesize:5
    }
    list:ListInt[]=[]//展示的内容的数据,接受后台返回的数据
}