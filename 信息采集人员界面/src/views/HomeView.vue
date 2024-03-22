<template>
  <div class="home">
    <el-container>
      <el-header>
        <el-row :gutter="20">
          <el-col :span="4"><img src="" class="logo" /></el-col>
          <el-col :span="14">
            <h2>交通治超执法管理系统</h2>
          </el-col>
          <el-col :span="2" class="col-btn">
            <div class="demo-type">
              <div>
                <el-button @click="GetPolice">
                  <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
                </el-button>
              </div>
            </div>
          </el-col>
          <el-col :span="4" class="col-btn"><el-button @click="delToken">退出登录</el-button></el-col>
        </el-row></el-header>
      <el-container>
        <el-aside width="200px"><el-menu active-text-color="#ffd04b" background-color="#545c64"
            class="el-menu-vertical-demo" :default-active="active" text-color="#fff" router>

            <!-- router开启路由模式，通过el-menu-item index来进行跳转 -->
            <el-menu-item :index="item.path" v-for="item in list" :key="item.path">
              <span>{{ item.meta.title }}</span>
            </el-menu-item>
          </el-menu></el-aside>
        <el-main>
          <!-- 设置路由出口 -->
          <router-view></router-view>
        </el-main>
      </el-container>
    </el-container>
  </div>
  <el-dialog v-model="isAddShow" title="修改个人信息">
    <el-form>
      <el-form-item label="旧密码" label-width="60px">
        <el-input v-model="oldPassword" autocomplete="off" />
      </el-form-item>
      <el-form-item label="新密码" label-width="60px">
        <el-input v-model="active2.password" autocomplete="off" />
      </el-form-item>
      <el-form-item label="姓名" label-width="60px">
        <el-input v-model="active2.name" autocomplete="off" />
      </el-form-item>
      <el-form-item label="电话" label-width="60px">
        <el-input v-model="active2.phone" autocomplete="off" />
      </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button icon="edit" @click="AlterMessage">修改</el-button>
        <el-button  type="primary" @click="isAddShow = false">取消</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts">
import { loginStore } from "@/stores/login";
import { InitData } from "@/type/home";
import axios from "axios";
import { Action, ElMessage, ElMessageBox } from "element-plus";
import { defineComponent, reactive, ref, toRefs } from "vue";
import { useRouter, useRoute } from 'vue-router'
export default defineComponent({
  name: "HomeView",
  setup() {
    const login = loginStore();
    const oldPassword = ref("");
    const data = reactive(new InitData());
    const router = useRouter()
    const route = useRoute()
    const list = router.getRoutes().filter(v => v.meta.isShow)
    const delToken = () => {
      localStorage.removeItem('token')
      router.push('/login')
    }
    const GetPolice = async () => {
      data.isAddShow = true;
      const res = await axios.get(`http://localhost:5172/api/Admin/GetPoliceById?id=${login.policeId}`);
      if (res.status === 200) {
        console.log(res.data)
        data.active2= {
          id: login.policeId,
          name: res.data.name,
          password: "",
          phone: res.data.phone,
        }
      }
    };
    const AlterMessage = async () => {
      debugger
      if (oldPassword.value != login.password) {
        ElMessageBox.alert('旧密码错误', '错误提示', {
          // if you want to disable its autofocus
          // autofocus: false,
          confirmButtonText: '确认',
          callback: (action: Action) => {
            ElMessage({
              type: 'info',
              message: `action: ${action}`,
            })
          },
        })
        return;
      } 
     const res =  await axios.put(`http://localhost:5172/api/Gather/UpdatePolice?id=${data.active2.id}&name=${data.active2.name}&password=${data.active2.password}&phone=${data.active2.phone}`);
      login.password = res.data.password;
      data.isAddShow = false
    };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const hasChildren = (item: { children: string | any[]; }) => {
      return item.children && item.children.length > 0;
    }
    return { ...toRefs(data),oldPassword,GetPolice ,AlterMessage, hasChildren, list, active: route.path, delToken }
  },
  components: {},
});
</script>
<style lang="scss" scoped>
.el-header {
  height: 80px;
  background-color: #666;

  .logo {
    height: 80px;
  }

  h2,
  .quit-login {
    text-align: center;
    height: 80px;
    line-height: 80px;
    color: #fff;
  }

  .col-btn {
    height: 80px;
    line-height: 80px;
  }
}

.el-aside {
  .el-menu {
    height: calc(100vh - 80px);
  }
}
.demo-type {
  display: flex;
}

.demo-type>div {
  flex: 1;
  text-align: center;
}

.demo-type>div:not(:last-child) {
  border-right: 1px solid var(--el-border-color);
}
</style>
