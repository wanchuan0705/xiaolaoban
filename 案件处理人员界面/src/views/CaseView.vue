<template>
    <div>
        <el-form :inline="true" :model="selectData" class="demo-form-inline">
            <el-form-item label="状态">
                <el-select v-model="selectData.status" placeholder="请选择状态" :style="{ width: '200px' }" clearable>
                    <el-option label="未审核" value="未审核"></el-option>
                    <el-option label="已审核" value="已审核"></el-option>
                    <el-option label="已立案" value="已立案"></el-option>
                    <el-option label="处理中" value="处理中"></el-option>
                    <el-option label="已完成" value="已完成"></el-option>
                    <el-option label="已结案" value="已结案"></el-option>
                    <el-option label="已驳回" value="已驳回"></el-option>
                    <el-option label="不立案" value="不立案"></el-option>
                    <el-option label="已撤销" value="已撤销"></el-option>
                </el-select>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="onSubmit" icon="search">查询</el-button>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="addClick"><el-icon>
                        <CirclePlus />
                    </el-icon>新增</el-button>
            </el-form-item>
        </el-form>
        <el-table :data="dataList.comList" border style="width: 100%">
            <el-table-column label="操作">
                <template #default="scope">
                    <!-- <el-button link type="primary" icon="edit" size="small" @click="changeUser(scope.row)">
                        编辑
                    </el-button> -->
                    <el-button link type="primary" icon="edit" size="small" @click="Check(scope.row)"
                        v-if="scope.row.State === '已审核'">
                        审核
                    </el-button>
                    <el-button link type="primary" icon="edit" size="small" @click="rollBack(scope.row)"
                        v-if="scope.row.State === '已完成'">
                        回退
                    </el-button>
                    <el-button link type="primary" icon="edit" size="small" @click="Check2(scope.row)"
                        v-if="scope.row.State === '已立案' || scope.row.State === '处理中'">
                        审理
                    </el-button>
                </template>
            </el-table-column>
            <el-table-column prop="Id" label="ID" />
            <el-table-column prop="CaseNo" label="CaseNo" />
            <el-table-column prop="State" label="状态" />
            <el-table-column prop="Address" label="Address" />
            <el-table-column prop="LawTypeId" label="LawTypeId" />
            <el-table-column prop="Types" label="Types" />
            <el-table-column prop="Details" label="Details" />
            <el-table-column prop="Platenumber" label="Platenumber" />
            <el-table-column prop="Date" label="Date" />
            <el-table-column prop="Phone" label="Phone" />
            <el-table-column prop="Image" label="Image" />
            <el-table-column prop="PolicerName" label="PolicerName" />
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
            <el-form-item label="CaseNo" label-width="50px">
                <el-input v-model="active.CaseNo" autocomplete="off" />
            </el-form-item>
            <el-form-item label="ViolatorsName" label-width="50px">
                <el-input v-model="active.ViolatorsName" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Phone" label-width="50px">
                <el-input v-model="active.Phone" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Address" label-width="50px">
                <el-input v-model="active.Address" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Details" label-width="50px">
                <el-input v-model="active.Details" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Platenumber" label-width="50px">
                <el-input v-model="active.Platenumber" autocomplete="off" />
            </el-form-item>
            <!-- <el-form-item label="Date" label-width="50px">
                <el-date-picker v-model="active.Date" type="datetime" placeholder="Select date and time"
                    value-format="yyyy-MM-dd HH:mm:ss" :editable="false" />
            </el-form-item> -->
            <el-form-item label="Image" label-width="50px">
                <el-input v-model="active.Image" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Description" label-width="50px">
                <el-input v-model="active.Description" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Model" label-width="50px">
                <el-input v-model="active.Model" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Content" label-width="50px">
                <el-input v-model="active.Content" autocomplete="off" />
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

    <el-dialog v-model="isCancelShow" title="审核案件">
        <el-form :model="active">
            <el-form-item label="是否接收" label-width="100px">
                <el-switch v-model="isCheck" />
            </el-form-item>
            <el-form-item label="原因" label-width="100px">
                <el-input v-model="active.Content" autocomplete="off" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="SaveOrderTake"><el-icon><Select /></el-icon>确认</el-button>
                <el-button type="primary" @click="isCancelShow = false">取消</el-button>
            </span>
        </template>
    </el-dialog>
    <el-dialog v-model="isCheck2" title="审理案件">
        <el-form :model="active2">
            <el-form-item label="法律" label-width="100px">
                <el-select v-model="active2.Id" placeholder="请选择法律">
                    <el-option v-for="law in lawData" :key="law.Id" :label="law.Content" :value="law.Id"></el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="content" label-width="100px">
                <el-input v-model="content0" autocomplete="off" />
            </el-form-item>
            <el-form-item label="content1" label-width="100px">
                <el-input v-model="content1" autocomplete="off" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="AddCaseLaw"><el-icon><Select /></el-icon>确认</el-button>
                <el-button type="primary" @click="isCheck2 = false">取消</el-button>
            </span>
        </template>
    </el-dialog>
    <el-dialog v-model="isRollBackShow" title="审核案件">
        <el-form :model="active">
            <el-form-item label="原因" label-width="100px">
                <el-input v-model="active.Content" autocomplete="off" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="SaveOrderTake12"><el-icon><Select /></el-icon>确认</el-button>
                <el-button type="primary" @click="isRollBackShow = false">取消</el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script lang="ts">
import axios from "axios";
import { ElNotification } from "element-plus";
import { computed, defineComponent, onMounted, reactive, ref, toRefs, watch } from "vue";
import { InitData, ListInt } from "../type/case";
import { loginStore } from "@/stores/login";
export default defineComponent({
    setup() {
        const isCheck = ref(false);
        const data = reactive(new InitData());
        onMounted(() => {
            getCase();
        });
        const rollBack = (row: ListInt) => {
            caseId.value = row.Id;
            data.isRollBackShow = true;
            
        };
        const content0 = ref("")
        const content1 = ref("")
        const AddCaseLaw = async () => {
            try {
                const response = await axios.post('http://localhost:5172/api/BackendControlr/AddCaseLaw', null, {
                    params: {
                        caseId: caseId.value,
                        lawId: data.active2.Id
                    }
                });
                debugger
                await axios.put(`http://localhost:5172/api/BackendControlr/handleCase?caseId=${caseId.value}&lawId=${data.active2.Id}&content=${content0.value}&content1=${content0.value}`);
                console.log(response);
            } catch (error) {
                console.error(error);
            }
            data.isAddShow = false,
                ElNotification.success({
                    title: '已完成',
                    message: '添加成功',
                    offset: 100,
                })
            data.isCheck2 = false;
            getCase();
        };
        const getCase = async () => {
            try {
                const login = loginStore();
                const { policeId } = login
                const res = await axios.get(`http://localhost:5172/api/BackendControlr/GetCase?PoliceId=${policeId}`);
                
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
                return data.list.slice(
                    (data.selectData.page - 1) * data.selectData.pagesize, //page=1 -->0,page=2-->10
                    data.selectData.page * data.selectData.pagesize //page=1-->10,page=2-->20
                );
            }),
        });
        const caseId = ref(0);
        const Check = (row: ListInt) => {
            caseId.value = row.Id;
            data.isCancelShow = true;
        };
        const Check2 = async (row: ListInt) => {
            caseId.value = row.Id;
            data.isCheck2 = true;
            const response = await axios.get(`http://localhost:5172/api/BackendControlr/GetLaw?lawtypeId=${row.LawTypeId}`);
            data.lawData = response.data;
            console.log(response)
        };
        const SaveOrderTake12 = async () => {
            try {
                debugger
                const response = await axios.post('http://localhost:5172/api/BackendControlr/SaveOrderTake12', null, {
                    params: {
                        caseId: caseId.value,
                        m_Content: data.active.Content
                    }
                });
                console.log(response);
            } catch (error) {
                console.error(error);
            }
            data.isAddShow = false,
                ElNotification.success({
                    title: '已完成',
                    message: '添加成功',
                    offset: 100,
                })
            data.isRollBackShow = false;
            getCase();
        }
        const SaveOrderTake = async () => {
            // const requestData = {
            //     caseId: data.active.Id,
            //     m_Content: data.active.Content
            // };
            debugger
            if (isCheck.value == true) {
                try {
                    const response = await axios.post('http://localhost:5172/api/BackendControlr/SaveOrderTake', null, {
                        params: {
                            caseId: caseId.value,
                            m_Content: data.active.Content
                        }
                    });
                    console.log(response);
                } catch (error) {
                    console.error(error);
                }
                data.isAddShow = false,
                    ElNotification.success({
                        title: '已完成',
                        message: '添加成功',
                        offset: 100,
                    })
            }
            else {
                try {
                    const response = await axios.post('http://localhost:5172/api/BackendControlr/SaveOrderTake11', null, {
                        params: {
                            caseId: caseId.value,
                            m_Content: data.active.Content
                        }
                    });
                    console.log(response);
                } catch (error) {
                    console.error(error);
                }
                data.isAddShow = false,
                    ElNotification.success({
                        title: '已完成',
                        message: '添加成功',
                        offset: 100,
                    })
            }
            data.isCancelShow = false;
            getCase();
        };
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
                LegalArticles: "",
                LawTypeId: 0
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
        const Cancel = (row: ListInt) => {
            data.isCancelShow = true;
            data.active.Content = row.Content;
            data.active.Id = row.Id;
        };
        const CancelCase = async () => {
            
            const obj = {
                id: data.active.Id,
                content: data.active.Content
            };
            // 发送 PUT 请求
            await axios.put('http://localhost:5172/api/Gather/CancelCase', obj);
            data.isCancelShow=true
        };
        const changeUser = (row: ListInt) => {
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
                LegalArticles: row.LegalArticles, // 补充 LegalArticles
                LawTypeId: row.LawTypeId
            }
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
        const addUser = async () => {
            data.list.push(data.active)
            
            const login = loginStore();
            const { policeId } = login
            const res = await axios.get(`http://localhost:5172/api/Admin/GetPoliceById?Id=${policeId}`);
            
            // 处理响应
            const requestData = {
                policeId: policeId,
                policeName: res.data.name,
                caseNo: data.active.CaseNo,
                violatorsName: data.active.ViolatorsName,
                phone: data.active.Phone,
                address: data.active.Address,
                details: data.active.Details,
                platenumber: data.active.Platenumber,
                date: data.active.Date,
                image: data.active.Image,
                description: data.active.Description,
                model: data.active.Model,
                content: data.active.Content,
                violatorsPhone: data.active.ViolatorsPhone
            };
                
            try {
                const response = await axios.post('http://localhost:5172/api/Gather/AddCase', requestData, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                });
                console.log(response);
            } catch (error) {
                console.error(error);
            }
            // data.isAddShow = false,
                ElNotification.success({
                    title: '已完成',
                    message: '添加成功',
                    offset: 100,
                })
            getCase();
        }
        return { ...toRefs(data), rollBack, content0, content1, AddCaseLaw, Check2, isCheck, SaveOrderTake, SaveOrderTake12, Check, Cancel,CancelCase,dataList, currentChange, sizeChange, addClick, addUser, onSubmit, changeUser, updateUser };
    },
});
</script>

<style scoped></style>