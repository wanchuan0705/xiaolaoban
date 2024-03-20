<template>
  <div>
    <el-form :inline="true" :model="selectData" class="demo-form-inline">
      <el-form-item label="姓名">
        <el-input v-model="selectData.Name" placeholder="请输入姓名" clearable />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">查询</el-button>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="setJudgeAdd(1)">新增后台案件处理人员</el-button>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="setJudgeAdd(2)">新增信息采集处理人员</el-button>
      </el-form-item>
    </el-form>
    <el-table :data="list" border style="width: 100%">
      <el-table-column prop="role" label="操作" width="180">
        <template #default="scope">
          <el-button link type="primary" size="small" @click="EditUser(scope.row)">
            编辑
          </el-button>
          <el-button link type="primary" size="small" @click="DeleteUser(scope.row)">
            删除
          </el-button>
        </template>
      </el-table-column>
      <el-table-column prop="Id" label="ID" v-if="false" />
      <el-table-column prop="Account" label="账号" />
      <el-table-column prop="Password" label="密码" />
      <el-table-column prop="Phone" label="电话" />
      <el-table-column prop="Name" label="姓名" />
      <el-table-column prop="Age" label="年龄" width="180" />
      <el-table-column prop="Sex" label="性别" width="180" />

    </el-table>
  </div>
  <el-dialog v-model="isAddShow" title="新增信息">
    <el-form :model="active">
      <el-form-item label="账号" label-width="50px">
        <el-input v-model="active.Account" autocomplete="off" />
      </el-form-item>
      <el-form-item label="密码" label-width="50px">
        <el-input v-model="active.Password" autocomplete="off" />
      </el-form-item>
      <el-form-item label="电话" label-width="50px">
        <el-input v-model="active.Phone" autocomplete="off" />
      </el-form-item>
      <el-form-item label="姓名" label-width="50px">
        <el-input v-model="active.Name" autocomplete="off" />
      </el-form-item>
      <el-form-item label="年龄" label-width="50px">
        <el-input v-model="active.Age" autocomplete="off" />
      </el-form-item>
      <el-form-item label="性别" label-width="50px">
        <el-select v-model="active.Sex" placeholder="请选择性别" autocomplete="off">
          <el-option label="未知" value="未知"></el-option>
          <el-option label="男" value="男"></el-option>
          <el-option label="女" value="女"></el-option>
        </el-select>
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
      <el-form-item label="账号" label-width="50px">
        <el-input v-model="active.Account" autocomplete="off" />
      </el-form-item>
      <el-form-item label="密码" label-width="50px">
        <el-input v-model="active.Password" autocomplete="off" />
      </el-form-item>
      <el-form-item label="电话" label-width="50px">
        <el-input v-model="active.Phone" autocomplete="off" />
      </el-form-item>
      <el-form-item label="姓名" label-width="50px">
        <el-input v-model="active.Name" autocomplete="off" />
      </el-form-item>
      <el-form-item label="年龄" label-width="50px">
        <el-input v-model="active.Age" autocomplete="off" />
      </el-form-item>
      <el-form-item label="性别" label-width="50px">
        <el-select v-model="active.Sex" placeholder="请选择性别" autocomplete="off">
          <el-option label="未知" value="未知"></el-option>
          <el-option label="男" value="男"></el-option>
          <el-option label="女" value="女"></el-option>
        </el-select>
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
import {  ElNotification } from "element-plus";
import { defineComponent, onMounted, reactive, toRefs, watch } from "vue";
import { InitData, ListInt } from "../type/user";
export default defineComponent({
  setup() {
    const data = reactive(new InitData());
    onMounted(() => {
      getUser();
    });
    const getUser = async () => {
      try {
        const res = await axios.get('http://localhost:5172/api/Admin/GetPolice');
        // 处理响应
        console.log(res.data); // 假设响应返回数据
        if (res && res.data) {
          // 确保 res 和 res.data 都存在
          data.list = res.data;
          console.log(data.list);
        }
      } catch (error) {
        console.error("Error fetching goods:", error);
      }
    };
   const setJudgeAdd =(value: number)=> {
     data.judgeAdd = value;
     addClick();
    }
    const addClick = () => {
      data.isAddShow = true; // 更新 isAddShow 的值
      data.active = {
        Id: 0,
        Account: "",
        Password: "",
        Phone: "",
        Name: "",
        Age: "",
        Sex: "",
        Role: ""
      };
    };
    const onSubmit = () => {
      let arr: ListInt[] = []; //定义数组，用来接受查询过后要展示的数据
      if (data.selectData.Name) {
        //判断两个是否其中一个有值
        if (data.selectData.Name) {
          arr = data.list.filter((value) => {
            //将过滤出来数组赋值给arr
            return value.Name.indexOf(data.selectData.Name) !== -1;
          });
        }
      } else {
        arr = data.list;
      }
      data.list = arr;
    };
    // 监听输入框的两个属性
    watch(() => data.selectData.Name, () => {
      if (data.selectData.Name == "") {
        getUser();
      }
    });
    const EditUser = (row: ListInt) => {
      console.log(row);
      data.active = {
        Id: row.Id,
        Account: row.Account,
        Password: row.Password,
        Phone: row.Phone,
        Name: row.Name,
        Age: row.Age,
        Sex: row.Sex,
        Role: row.Role
      }
      data.isShow = true
    }
    const DeleteUser = async (row: ListInt) => {
      console.log(row);
      // 发送 DELETE 请求
      await axios.delete(`http://localhost:5172/api/Admin/DeletPolice?id=${row.Id}`);
      ElNotification.success({
        title: '已完成',
        message: '删除成功',
        offset: 100,
      })
    }
    const updateUser = async () => {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      let obj: any = data.list.find((value) => value.Id == data.active.Id)
      obj.Account = data.active.Account
      obj.Password = data.active.Password
      obj.Phone = data.active.Phone
      obj.Name = data.active.Name
      obj.Age = data.active.Age
      obj.Sex = data.active.Sex
      console.log(obj);
      data.list.forEach((item, i) => {
        if (item.Id == obj.id) {
          data.list[i] = obj
        }
      })
      // 发送 PUT 请求
      await axios.put('http://localhost:5172/api/Admin/PutPolice', obj);
      data.isShow = false,
        ElNotification.success({
          title: '已完成',
          message: '更新成功',
          offset: 100,
        })
    }
    const addUser = async () => {
      data.list.push(data.active)
      data.isAddShow = false;
      let queryParams = '';
      // 遍历 data.list 数组中的对象，并构建查询参数
      for (const item of data.list) {
        for (const key in item) {
          if (Object.prototype.hasOwnProperty.call(item, key) && key !== 'Id' && key !== 'Role') {
            queryParams += `&${key}=${encodeURIComponent(item[key as keyof typeof item] as string)}`;
          }
        }
      }
      let response;
      if (data.judgeAdd == 1) {
         response = await axios.post(`http://localhost:5172/api/Admin/AddUserRole1?${queryParams}`);
      }
      else {
         response = await axios.post(`http://localhost:5172/api/Admin/AddUserRole2?${queryParams}`);
      }
      console.log(response);
        ElNotification.success({
          title: '已完成',
          message: '添加成功',
          offset: 100,
        })
    }
    return { ...toRefs(data), DeleteUser, setJudgeAdd, addClick, addUser, onSubmit, EditUser, updateUser };
  },
});
</script>

<style scoped></style>