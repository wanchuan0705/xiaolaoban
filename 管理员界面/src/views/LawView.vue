<template>
    <div>
        <el-form :inline="true" :model="selectData" class="demo-form-inline">
            <el-form-item label="ID">
                <el-input v-model="selectData.Id" placeholder="请输入ID" clearable />
            </el-form-item>
            <el-form-item>
                <el-button type="primary" icon="search" @click="onSubmit">
                    查询
                </el-button>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="addClick"><el-icon>
                        <CirclePlus />
                    </el-icon>新增</el-button>
            </el-form-item>
        </el-form>
        <el-table :data="dataList.comList" border style="width: 100%">
            <el-table-column prop="Id" label="ID" width="180" />
            <el-table-column prop="Content" label="内容" width="180" />
            <el-table-column label="操作">
                <template #default="scope">
                    <el-button link type="primary" icon="Edit" size="small" @click="changeUser(scope.row)">
                        编辑
                    </el-button>
                    <!-- <el-button link type="primary" icon="delete" size="small" @click="changeUser(scope.row)">
                        删除
                    </el-button> -->
                </template>
            </el-table-column>
        </el-table>
        <el-pagination @current-change="currentChange" @size-change="sizeChange" layout="prev, pager, next"
            :total="selectData.count * 2" />
    </div>
    <el-dialog v-model="isAddShow" title="新增信息">
        <el-form :model="active">
            <el-form-item label="内容" label-width="50px">
                <el-input v-model="active.Content" autocomplete="off" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="addUser"><el-icon><Select /></el-icon>新增</el-button>
                <el-button type="primary" @click="isAddShow = false"><el-icon>
                        <CircleClose />
                    </el-icon>取消</el-button>
            </span>
        </template>
    </el-dialog>
    <el-dialog v-model="isShow" title="编辑信息">
        <el-form :model="active">
            <el-form-item label="内容" label-width="50px">
                <el-input v-model="active.Content" autocomplete="off" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="updateUser"><el-icon><Select /></el-icon>更改</el-button>
                <el-button type="primary" @click="isShow = false"><el-icon>
                        <CircleClose />
                    </el-icon>取消</el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script lang="ts">
import axios from "axios";
import { ElNotification } from "element-plus";
import { computed, defineComponent, onMounted, reactive, toRefs, watch } from "vue";
import { InitData, ListInt } from "../type/law";
export default defineComponent({
    setup() {
        const data = reactive(new InitData());
        onMounted(() => {
            getUser();
        });
        const getUser = async () => {
            try {
                const res = await axios.get('http://localhost:5172/api/Admin/GetLaws');
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
                Id: 0,
                Content: "",
            };
        };
        const onSubmit = () => {
            let arr: ListInt[] = []; //定义数组，用来接受查询过后要展示的数据
            if (data.selectData.Id) {
                //判断是否其中一个有值
                if (data.selectData.Id) {
                    arr = data.list.filter((value) => {
                        //将过滤出来数组赋值给arr
                        return value.Id === data.selectData.Id;
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
        watch(() => data.selectData.Directory, () => {
            if (data.selectData.Directory == "") {
                getUser();
            }
        });
        const changeUser = (row: ListInt) => {
            console.log(row);
            data.active = {
                Id: row.Id,
                Content: row.Content,
            }
            data.isShow = true

        }
        const updateUser = async () => {
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            let obj: any = data.list.find((value) => value.Id == data.active.Id)
            obj.Content = data.active.Content
            console.log(obj);
            data.list.forEach((item, i) => {
                if (item.Id == obj.id) {
                    data.list[i] = obj
                }
            })
            await axios.put(`http://localhost:5172/api/Admin/UpdateLawContent?id=${obj.Id}&newContent=${obj.Content}`);
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