<template>
    <div>
        <el-form :inline="true" :model="selectData" class="demo-form-inline">
            <el-form-item label="状态">
                <el-select v-model="selectData.status" placeholder="请选择状态" :style="{ width: '200px' }" clearable>
                    <el-option label="未审核" value="待受理"></el-option>
                    <el-option label="已审核" value="待受理"></el-option>
                    <el-option label="已立案" value="已驳回"></el-option>
                    <el-option label="处理中" value="处理中"></el-option>
                    <el-option label="已完成" value="已完结"></el-option>
                    <el-option label="已结案" value="待受理"></el-option>
                    <el-option label="已驳回" value="待受理"></el-option>
                    <el-option label="不立案" value="待受理"></el-option>
                    <el-option label="已撤销" value="待受理"></el-option>
                </el-select>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="onSubmit" icon="search">查询</el-button>
            </el-form-item>
        </el-form>
        <el-table :data="dataList.comList" border style="width: 100%">
            <el-table-column label="操作">
                <template #default="scope">
                    <el-button link type="primary" icon="edit" size="small" @click="changeUser(scope.row)">
                        编辑
                    </el-button>
                </template>
            </el-table-column>
            <el-table-column prop="Id" label="ID" />
            <el-table-column prop="CaseNo" label="CaseNo" />
            <el-table-column prop="Address" label="Address" />
            <el-table-column prop="Types" label="Types" />
            <el-table-column prop="Details" label="Details" />
            <el-table-column prop="Platenumber" label="Platenumber" />
            <el-table-column prop="Date" label="Date" />
            <el-table-column prop="Phone" label="Phone" />
            <el-table-column prop="Image" label="Image" />
            <el-table-column prop="PolicerName" label="PolicerName" />
            <el-table-column prop="State" label="State" />
            <el-table-column prop="ViolatorsName" label="ViolatorsName" />
            <el-table-column prop="ApplicationTime" label="ApplicationTime" />
            <el-table-column prop="Description" label="Description" />
            <el-table-column prop="Model" label="Model" />
            <el-table-column prop="Judgment" label="Judgment" />
            <el-table-column prop="M_Conten" label="M_Conten" />
            <el-table-column prop="PolicerName1" label="PolicerName1" />
            <el-table-column prop="ViolatorsPhone" label="ViolatorsPhone" />
            <el-table-column prop="OrderTake" label="OrderTake" />
            <el-table-column prop="LegalArticles" label="LegalArticles" />


        </el-table>
        <el-pagination @current-change="currentChange" @size-change="sizeChange" layout="prev, pager, next"
            :total="selectData.count * 2" />
    </div>
    <el-dialog v-model="isAddShow" title="新增信息">
        <el-form :model="active">
            <el-form-item label="Address" label-width="50px">
                <el-input v-model="active.Address" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Details" label-width="50px">
                <el-input v-model="active.Details" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Types" label-width="50px">
                <el-dropdown size="large" split-button type="primary">
                    <span>{{ active.Types }}</span> <!-- 显示当前选中的法律类型的名称 -->
                    <template #dropdown>
                        <el-dropdown-menu>
                            <el-dropdown-item v-for="type in Types" :key="type.Id" @click="selectLawType(type)">
                                {{ type.Name }}
                            </el-dropdown-item>
                        </el-dropdown-menu>
                    </template>
                </el-dropdown>
            </el-form-item>
            <el-form-item label="Platenumber" label-width="50px">
                <el-input v-model="active.Platenumber" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Date" label-width="50px">
                <el-input v-model="active.Date" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Phone" label-width="50px">
                <el-input v-model="active.Phone" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Image" label-width="50px">
                <el-input v-model="active.Image" autocomplete="off" />
            </el-form-item>
            <el-form-item label="PolicerName" label-width="50px">
                <el-input v-model="active.PolicerName" autocomplete="off" />
            </el-form-item>
            <el-form-item label="State" label-width="50px">
                <el-select v-model="active.State" placeholder="请选择状态" autocomplete="off">
                    <el-option label="未审核" value="待受理"></el-option>
                    <el-option label="已审核" value="待受理"></el-option>
                    <el-option label="已立案" value="已驳回"></el-option>
                    <el-option label="处理中" value="处理中"></el-option>
                    <el-option label="已完成" value="已完结"></el-option>
                    <el-option label="已结案" value="待受理"></el-option>
                    <el-option label="已驳回" value="待受理"></el-option>
                    <el-option label="不立案" value="待受理"></el-option>
                    <el-option label="已撤销" value="待受理"></el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="ViolatorsName" label-width="50px">
                <el-input v-model="active.ViolatorsName" autocomplete="off" />
            </el-form-item>
            <el-form-item label="ApplicationTime" label-width="50px">
                <el-input v-model="active.ApplicationTime" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Description" label-width="50px">
                <el-input v-model="active.Description" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Model" label-width="50px">
                <el-input v-model="active.Model" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Judgment" label-width="50px">
                <el-input v-model="active.Judgment" autocomplete="off" />
            </el-form-item>
            <el-form-item label="M_Conten" label-width="50px">
                <el-input v-model="active.M_Content" autocomplete="off" />
            </el-form-item>
            <el-form-item label="PolicerName1" label-width="50px">
                <el-input v-model="active.PolicerName1" autocomplete="off" />
            </el-form-item>
            <el-form-item label="ViolatorsPhone" label-width="50px">
                <el-input v-model="active.ViolatorsPhone" autocomplete="off" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="addUser">新增</el-button>
                <el-button type="primary" @click="isAddShow = false">取消</el-button>
            </span>
        </template>
    </el-dialog>
    <el-dialog v-model="isShow" title="编辑信息">
        <el-form :model="active">
            <el-form-item label="法律类型" label-width="100px">
                <el-dropdown size="large" split-button type="primary">
                    <span>{{ selectedLawType.Name }}</span> <!-- 显示当前选中的法律类型的名称 -->
                    <template #dropdown>
                        <el-dropdown-menu>
                            <el-dropdown-item v-for="type in Types" :key="type.Id" @click="selectLawType(type)">
                                {{ type.Name }}
                            </el-dropdown-item>
                        </el-dropdown-menu>
                    </template>
                </el-dropdown>
            </el-form-item>
            
            <el-form-item label="PolicerName1" label-width="50px">
                <el-dropdown size="large" split-button type="primary">
                    <span>{{ selectedPolicerName1.Name }}</span> <!-- 显示当前选中的法律类型的名称 -->
                    <template #dropdown>
                        <el-dropdown-menu>
                            <el-dropdown-item v-for="type in PolicerName1" :key="type.Id" @click="selectLawType(type)">
                                {{ type.Name }}
                            </el-dropdown-item>
                        </el-dropdown-menu>
                    </template>
                </el-dropdown>
            </el-form-item>
            
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="updateUser">更改</el-button>
                <el-button type="primary" @click="isShow = false">取消</el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script lang="ts">
import axios from "axios";
import { ElNotification } from "element-plus";
import { computed, defineComponent, onMounted, reactive, ref, toRefs, watch } from "vue";
import { InitData, ListInt } from "../type/case";
export default defineComponent({
    setup() {
        const data = reactive(new InitData());
        const Types = ref([{ Id: 0, Name: '' }]); // 存储从接口获取的法律类型数据
        const selectedLawType = ref({ Id: 0, Name: ""}); // 存储当前选中的法律类型
        const PolicerName1 = ref([{ Id: 0, Name: '' }]); 
        const selectedPolicerName1 = ref({ Id: 0, Name: ""}); 
        onMounted(() => {
            getCase();
        });
        const getCase = async () => {
            try {
                const res = await axios.get('http://localhost:5172/api/Admin/GetCases');
                // 处理响应
                console.log(res.data); // 假设响应返回数据
                if (res && res.data) {
                    // 确保 res 和 res.data 都存在
                    data.list = res.data;
                    data.selectData.count = res.data.length;
                    console.log(data.list);
                }
            } catch (error) {
                console.error("Error fetching goods:", error);
            }
        };
        const dataList = reactive({
            comList: computed(() => {
                // 1-->[1-10]
                // 2-->[11-20]
                // 3-->[21-30]
                // ...
                return data.list.slice(
                    (data.selectData.page - 1) * data.selectData.pagesize, //page=1 -->0,page=2-->10
                    data.selectData.page * data.selectData.pagesize //page=1-->10,page=2-->20
                    //   0-9
                    // 10-19
                );
            }),
        });
        const addClick = () => {
            data.isAddShow = true; // 更新 isAddShow 的值
            data.active = {
                CaseNo: "",
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
        };
        const onSubmit = () => {
            let arr: ListInt[] = []; //定义数组，用来接受查询过后要展示的数据
            if (data.selectData.status) {
                //判断两个是否其中一个有值
                if (data.selectData.status) {
                    arr = data.list.filter((value) => {
                        //将过滤出来数组赋值给arr
                        return value.State.indexOf(data.selectData.status) !== -1;
                    });
                }
            } else {
                arr = data.list;
            }
            data.list = arr;
        };
        const currentChange = (page: number) => {
            data.selectData.page = page;
        };
        const sizeChange = (pagesize: number) => {
            data.selectData.pagesize = pagesize;
        };
        // 监听输入框的两个属性
        watch(() => data.selectData.status, () => {
            if (data.selectData.status == "") {
                getCase();
            }
        });
        const selectLawType = (type: { Id: number; Name: string; }) => {
            selectedLawType.value = type; // 更新当前选中的法律类型
        };
        const selectPolicerName1 = (type: { Id: number; Name: string; }) => {
            selectedPolicerName1.value = type; // 更新当前选中的法律类型
        };
        const changeUser = async (row: ListInt) => {
            const resTypes = await axios.get('http://localhost:5172/api/Admin/GetLawType');
            Types.value = resTypes.data; // 将获取到的数据存储到 resTypes 中
            const resPolicerName1 = await axios.get('http://localhost:5172/api/Admin/GetPolice');
            PolicerName1.value = resPolicerName1.data; // 将获取到的数据存储到 resTypes 中
            console.log(row);
            data.active = {
                CaseNo: row.CaseNo,
                Id: row.Id,
                Address: row.Address,
                Details: row.Details,
                Types: row.Types,
                Platenumber: row.Platenumber,
                Date: row.Date,
                Phone: row.Phone,
                Image: row.Image,
                PolicerName: row.PolicerName,
                State: row.State,
                ViolatorsName: row.ViolatorsName,
                ApplicationTime: row.ApplicationTime,
                Description: row.Description,
                Model: row.Model,
                Judgment: row.Judgment,
                M_Content: row.M_Content,
                Content:row.Content,
                PolicerName1: row.PolicerName1,
                ViolatorsPhone: row.ViolatorsPhone,
                OrderTake: row.OrderTake, // 补充 OrderTake
                LegalArticles: row.LegalArticles // 补充 LegalArticles

            }
            selectedLawType.value.Name = data.active.Types,
            selectedPolicerName1.value.Name = data.active.PolicerName1,
            data.isShow = true

        }
        const updateUser = () => {
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            let obj: any = data.list.find((value) => value.Id == data.active.Id)
            obj.CaseNo = data.active.CaseNo
            obj.Address = data.active.Address
            obj.Details = data.active.Details
            obj.Types = data.active.Types
            obj.Platenumber = data.active.Platenumber
            obj.Date = data.active.Date
            obj.Phone = data.active.Phone
            obj.Image = data.active.Image
            obj.PolicerName = data.active.PolicerName
            obj.State = data.active.State
            obj.ViolatorsName = data.active.ViolatorsName
            obj.ApplicationTime = data.active.ApplicationTime
            obj.Description = data.active.Description
            obj.Model = data.active.Model
            obj.Judgment = data.active.Judgment
            obj.M_Conten = data.active.M_Content
            obj.Content = data.active.Content
            obj.PolicerName1 = data.active.PolicerName1
            obj.ViolatorsPhone = data.active.ViolatorsPhone
            obj.OrderTake = data.active.OrderTake
            obj.LegalArticles = data.active.LegalArticles
            console.log(obj);
            data.list.forEach((item, i) => {
                if (item.Id == obj.id) {
                    data.list[i] = obj
                }
            })
            data.isShow = false,
                ElNotification.success({
                    title: '已完成',
                    message: '更新成功',
                    offset: 100,
                })
        }
        const addUser = () => {
            data.list.push(data.active)
            
            data.isAddShow = false,
                ElNotification.success({
                    title: '已完成',
                    message: '添加成功',
                    offset: 100,
                })
        }
        return { ...toRefs(data), selectPolicerName1, selectedPolicerName1,PolicerName1 ,selectedLawType,selectLawType, Types,dataList, currentChange, sizeChange, addClick, addUser, onSubmit, changeUser, updateUser };
    },
});
</script>

<style scoped></style>