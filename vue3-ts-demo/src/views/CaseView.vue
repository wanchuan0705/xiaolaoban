<template>
    <div>
        <el-form :inline="true" :model="selectData" class="demo-form-inline">
            <el-form-item label="状态">
                <el-select v-model="selectData.status" placeholder="请选择状态" :style="{ width: '200px' }" clearable>
                    <el-option label="待受理" value="待受理"></el-option>
                    <el-option label="已驳回" value="已驳回"></el-option>
                    <el-option label="处理中" value="处理中"></el-option>
                    <el-option label="已完结" value="已完结"></el-option>
                </el-select>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="onSubmit">查询</el-button>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="addClick">新增</el-button>
            </el-form-item>
        </el-form>
        <el-table :data="dataList.comList" border style="width: 100%">
            <el-table-column prop="Id" label="ID" width="180" />
            <el-table-column prop="Address" label="Address" width="180" />
            <el-table-column prop="Types" label="Types" width="180" />
            <el-table-column prop="Details" label="Details" width="180" />
            <el-table-column prop="Platenumber" label="Platenumber" width="180" />
            <el-table-column prop="Date" label="Date" width="180" />
            <el-table-column prop="Phone" label="Phone" width="180" />
            <el-table-column prop="Image" label="Image" width="180" />
            <el-table-column prop="PolicerName" label="PolicerName" width="180" />
            <el-table-column prop="State" label="State" width="180" />
            <el-table-column prop="ViolatorsName" label="v" width="180" />
            <el-table-column prop="ApplicationTime" label="ApplicationTime" width="180" />
            <el-table-column prop="Description" label="Description" width="180" />
            <el-table-column prop="Model" label="Model" width="180" />
            <el-table-column prop="Judgment" label="Judgment" width="180" />
            <el-table-column prop="M_Conten" label="M_Conten" width="180" />
            <el-table-column prop="PolicerName1" label="PolicerName1" width="180" />
            <el-table-column prop="ViolatorsPhone" label="ViolatorsPhone" width="180" />
            <el-table-column label="操作">
                <template #default="scope">
                    <el-button link type="primary" size="small" @click="changeUser(scope.row)">
                        编辑
                    </el-button>
                </template>
            </el-table-column>
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
                <el-input v-model="active.Types" autocomplete="off" />
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
                    <el-option label="待受理" value="待受理"></el-option>
                    <el-option label="已驳回" value="已驳回"></el-option>
                    <el-option label="处理中" value="处理中"></el-option>
                    <el-option label="已完结" value="已完结"></el-option>
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
                <el-input v-model="active.M_Conten" autocomplete="off" />
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
            <el-form-item label="Address" label-width="50px">
                <el-input v-model="active.Address" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Details" label-width="50px">
                <el-input v-model="active.Details" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Types" label-width="50px">
                <el-input v-model="active.Types" autocomplete="off" />
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
                    <el-option label="待受理" value="待受理"></el-option>
                    <el-option label="已驳回" value="已驳回"></el-option>
                    <el-option label="处理中" value="处理中"></el-option>
                    <el-option label="已完结" value="已完结"></el-option>
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
                <el-input v-model="active.M_Conten" autocomplete="off" />
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
                <el-button @click="updateUser">更改</el-button>
                <el-button type="primary" @click="isShow = false">取消</el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script lang="ts">
import axios from "axios";
import { ElNotification } from "element-plus";
import { computed, defineComponent, onMounted, reactive, toRefs, watch } from "vue";
import { InitData, ListInt } from "../type/case";
export default defineComponent({
    setup() {
        const data = reactive(new InitData());
        onMounted(() => {
            getCase();
        });
        const getCase = async () => {
            try {
                const res = await axios.get('https://console-mock.apipost.cn/mock/e2291285-0895-4edf-95ea-855e14b04157/?apipost_id=c6d16f');
                // 处理响应
                console.log(res.data); // 假设响应返回数据
                if (res && res.data) {
                    // 确保 res 和 res.data 都存在
                    data.list = res.data.data;
                    data.selectData.count = res.data.data.length;
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
        const changeUser = (row: ListInt) => {
            console.log(row);
            data.active = {
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
                M_Conten: row.M_Conten,
                PolicerName1: row.PolicerName1,
                ViolatorsPhone: row.ViolatorsPhone,
            }
            data.isShow = true

        }
        const updateUser = () => {
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            let obj: any = data.list.find((value) => value.Id == data.active.Id)
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
            obj.M_Conten = data.active.M_Conten
            obj.PolicerName1 = data.active.PolicerName1
            obj.ViolatorsPhone = data.active.ViolatorsPhone
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
        return { ...toRefs(data), dataList, currentChange, sizeChange, addClick, addUser, onSubmit, changeUser, updateUser };
    },
});
</script>

<style scoped></style>