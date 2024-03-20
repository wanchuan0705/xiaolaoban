export interface ListInt {
    name: string
    roleId: number
    roleList?: ListInt[]
    viewRole?: string
}
export class InitData {
    id: number
    authority: number[]
    constructor(id: number, authority: number[]) {
        this.id = id
        this.authority = authority
    }
    list:ListInt[]=[]
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    treeRef:any
}